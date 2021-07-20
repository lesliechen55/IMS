using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Freight.Data;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Freight.Service.Imp
{
    public class ExportService : IExportService
    {
        private readonly CarrierContext _dbContext;

        public ExportService(CarrierContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<ExportCarrierOutput>> GetCarrierInfoAsync()
        {
            var list = await (from tcompany in _dbContext.Tcompany
                              join tchannel in _dbContext.Tchannel on tcompany.FcompanyId equals tchannel.FcompanyId
                              join tchannelFreight in _dbContext.TchannelFreight on tchannel.FchannelId equals tchannelFreight
                                  .FchannelId
                              where tcompany.FcheckState == 1 &&
                                    (tchannelFreight.FupdateTime < DateTime.UtcNow &&
                                     tchannelFreight.FupdateTime >= DateTime.UtcNow.AddDays(-7))
                              group tcompany by new { tcompany.FcompanyId, tcompany.FcompanyName } into g
                              where g.Count() > 0
                              select g.Key).ProjectTo<ExportCarrierOutput>().ToListAsync();
            return list;
        }

        public async Task<IEnumerable<ExportValidChannelOutput>> GetChannelInfoAsync()
        {
            var list = await (from tchannel in _dbContext.Tchannel
                              join tchannelFreight in _dbContext.TchannelFreight on tchannel.FchannelId equals tchannelFreight.FchannelId
                              join tcompany in _dbContext.Tcompany on tchannel.FcompanyId equals tcompany.FcompanyId
                              where tchannel.Fstate == 1 &&
                                    tchannel.DelFlag == false &&
                                    tchannel.FexpireTime > DateTime.UtcNow &&
                                    tcompany.FcheckState == 1
                              select new
                              {
                                  tchannel.FchannelTitle,
                                  tchannel.FproductType,
                                  tchannel.FminDay,
                                  tchannel.FmaxDay,
                                  tchannel.Fcountrys,
                                  tchannelFreight.FlimitWeight,
                                  tchannelFreight.FoperateCost,
                                  tchannelFreight.FfirstWeight,
                                  tchannelFreight.FfirstPrice,
                                  tchannelFreight.FfreightIntervals,
                                  tcompany.FcompanyName,
                                  tchannelFreight.FfreightType,
                                  FChannelType = tchannel.FchannelSubTypeId.ToString().Substring(0, 1),
                                  tchannel.FchannelSubTypeId,
                                  tchannel.FpublishTime
                              }).ProjectTo<ExportValidChannelOutput>().ToListAsync();
            return list;
        }

        public async Task<IEnumerable<ExportInvalidChannelOutput>> GetInvalidChannelInfoAsync()
        {
            var list = await (from tchannel in _dbContext.Tchannel
                              join tchannelFreight in _dbContext.TchannelFreight on tchannel.FchannelId equals tchannelFreight.FchannelId
                              join tcompany in _dbContext.Tcompany on tchannel.FcompanyId equals tcompany.FcompanyId
                              where ((tchannel.Fstate == 2 && tchannel.FupdateTime > DateTime.UtcNow.AddDays(-7)) ||
                                     (tchannel.Fstate == 1 && tchannel.FexpireTime < DateTime.UtcNow && tchannel.FexpireTime > DateTime.UtcNow.AddDays(-7))) && tchannel.DelFlag == false && tcompany.FcheckState == 1
                              select new
                              {
                                  tchannel.FchannelTitle,
                                  tchannel.FproductType,
                                  tchannel.FminDay,
                                  tchannel.FmaxDay,
                                  tchannel.Fcountrys,
                                  tchannelFreight.FlimitWeight,
                                  tchannelFreight.FoperateCost,
                                  tchannelFreight.FfirstWeight,
                                  tchannelFreight.FfirstPrice,
                                  tchannelFreight.FfreightIntervals,
                                  tcompany.FcompanyName,
                                  tchannelFreight.FfreightType,
                                  tchannel.FexpireTime,
                                  FexpireType = tchannel.Fstate == 2 ? "手动失效" : "自动失效",
                                  FChannelType = tchannel.FchannelSubTypeId.ToString().Substring(0, 1),
                                  tchannel.FchannelSubTypeId,
                                  tchannel.FpublishTime
                              }).ProjectTo<ExportInvalidChannelOutput>().ToListAsync();
            return list;
        }

        public async Task<IEnumerable<InquiryPageDataOutput>> GetInquiryInfoAsync(long? id, string title, string inquiryNo, byte? status, string publisher, DateTime? publishStartTime, DateTime? publishEndTime, DateTime? expireStartTime, DateTime? expireEndTime)
        {
            var queryable = _dbContext.TinquiryOrder
                .WhereIf(() => id.HasValue && id.Value > 0, x => x.ForderId == id)
                .WhereIf(() => !title.IsNullOrWhiteSpace(), x => x.Ftitle.Contains(title))
                .WhereIf(() => !inquiryNo.IsNullOrWhiteSpace(), x => x.FinquiryOrderNo == inquiryNo)
                .WhereIf(() => status.HasValue, x => x.Fstatus == status.Value)
                .WhereIf(() => !publisher.IsNullOrWhiteSpace(), x => x.FuserUniqueId == publisher.Trim())
                .WhereIf(() => publishStartTime.HasValue, x => x.FcreateTime >= publishStartTime.Value.Date)
                .WhereIf(() => publishEndTime.HasValue, x => x.FcreateTime < publishEndTime.Value.AddDays(1).Date)
                .WhereIf(() => expireStartTime.HasValue, x => x.FexpireDate >= expireStartTime.Value.Date)
                .WhereIf(() => expireEndTime.HasValue, x => x.FexpireDate < expireEndTime.Value.AddDays(1).Date);
            var list = await queryable.OrderByDescending(x => x.FcreateTime)
                                    .ProjectTo<InquiryPageDataOutput>()
                                    .ToListAsync();
            return list;
        }
    }
}