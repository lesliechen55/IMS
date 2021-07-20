using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Freight.Data;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Freight.Service.Imp
{
    public class HomeService : IHomeService
    {
        private readonly CarrierContext _dbContext;

        public HomeService(CarrierContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(int totalChannel, int totalInquiry, int totalQuote, int totalCarrier)> GetMainDataAsync()
        {
            var totalChannel = await _dbContext.Tchannel.Where(x => !x.DelFlag).CountAsync();
            var totalInquiry = await _dbContext.TinquiryOrder.CountAsync();
            var totalQuote = await _dbContext.TquoteOrder.CountAsync();
            var totalCarrier = await _dbContext.Tcompany.Where(x => x.FcheckState == 1).CountAsync();
            return (totalChannel, totalInquiry, totalQuote, totalCarrier);
        }

        public async Task<(IEnumerable<ChannelPageDataOutput> outputs, int total)> GetChannelPageDataAsync(ChannelPageDataInput input)
        {
            var queryable = GetQueryable(input);

            var count = await queryable.CountAsync();

            var list = await queryable.ToPage(input.Page, input.Limit).ToListAsync();

            return (list, count);
        }

        private IQueryable<ChannelPageDataOutput> GetQueryable(ChannelPageDataInput input)
        {
            var query = _dbContext.Tchannel.Where(x => x.DelFlag == false)
               .WhereIf(() => input.Id.HasValue, x => x.FchannelId == input.Id.Value)
               .WhereIf(() => !input.Name.IsNullOrWhiteSpace(), x => x.FchannelTitle.Contains(input.Name.Trim()))
               .WhereIf(() => input.Status.HasValue, x => x.Fstate == (byte)input.Status.Value)
               .WhereIf(() => input.PublishStartTime.HasValue, x => x.FpublishTime >= input.PublishStartTime.Value.Date)
               .WhereIf(() => input.PublishEndTime.HasValue, x => x.FpublishTime < input.PublishEndTime.Value.AddDays(1).Date)
               .WhereIf(() => input.ExpireStartTime.HasValue, x => x.FexpireTime >= input.ExpireStartTime.Value.Date)
               .WhereIf(() => input.ExpireEndTime.HasValue, x => x.FexpireTime < input.ExpireEndTime.Value.AddDays(1).Date);


            var queryable = (from tchannel in query
                             join tcompany in _dbContext.Tcompany
                                 on tchannel.FcompanyId equals tcompany.FcompanyId
                             join tcompanyBusiness in _dbContext.TcompanyBusiness
                                 on tcompany.FcompanyId equals tcompanyBusiness.FcompanyId
                             join tchannelFreight in _dbContext.TchannelFreight
                                 on tchannel.FchannelId equals tchannelFreight.FchannelId
                             where tcompany.FcheckState == 1
                             orderby tchannel.FupdateTime descending,
                                 tchannel.FpublishTime descending,
                                 tchannel.FcreateTime descending
                             select new
                             {
                                 tchannel.FchannelTitle,
                                 tchannel.FchannelId,
                                 tchannel.FproductType,
                                 tchannel.FminDay,
                                 tchannel.FmaxDay,
                                 tcompanyBusiness.Fcitys,
                                 tchannel.Fcountrys,
                                 tchannelFreight.FlimitWeight,
                                 tchannelFreight.FoperateCost,
                                 tchannelFreight.FfirstWeight,
                                 tchannelFreight.FfirstPrice,
                                 tchannelFreight.FfreightType,
                                 tchannelFreight.FfreightIntervals,
                                 tcompany.FcompanyName,
                                 tchannel.FpublishTime,
                                 tchannel.Fstate,
                                 tchannel.FexpireTime,
                                 tchannel.FvalidReportTimes
                             }).ProjectTo<ChannelPageDataOutput>();
            return queryable;
        }

        public async Task<IEnumerable<ChannelPageDataOutput>> GetChannelExcelAsync(ChannelPageDataInput input)
        {
            var queryable = GetQueryable(input);
            var outputs = await queryable.ToListAsync();
            return outputs;
        }

        public async Task<(IEnumerable<QuotePageDataOutput> outputs, int total)> GetQuotePageDataAsync(QuotePageDataInput input)
        {
            var queryable = GetQueryable(input);

            var count = await queryable.CountAsync();

            var list = await queryable.ToPage(input.Page, input.Limit).ToListAsync();

            return (list, count);
        }

        public async Task<IEnumerable<QuotePageDataOutput>> GetQuotePageInfoAsync(QuotePageDataInput input)
        {
            var queryable = GetQueryable(input);
            var outputs = await queryable.ToListAsync();
            return outputs;
        }

        private IQueryable<QuotePageDataOutput> GetQueryable(QuotePageDataInput input)
        {
            var query = _dbContext.TquoteOrder.
                WhereIf(() => input.QuoteId.HasValue && input.QuoteId.Value > 0, x => x.FquoteId == input.QuoteId.Value)
                .WhereIf(() => !input.QuoteNo.IsNullOrWhiteSpace(), x => x.FquoteOrderNo == input.QuoteNo.Trim())
                .WhereIf(() => !input.InquiryNo.IsNullOrWhiteSpace(), x => x.FinquiryOrderNo == input.InquiryNo.Trim())
                .WhereIf(() => !input.CompanyName.IsNullOrWhiteSpace(), x => x.FcompanyName.Contains(input.CompanyName.Trim()))
                .WhereIf(() => input.CancelStatus.HasValue, x => x.Fcancel == input.CancelStatus.Value)
                .WhereIf(() => input.StartTime.HasValue, x => x.FcreateTime >= input.StartTime.Value.Date)
                .WhereIf(() => input.EndTime.HasValue, x => x.FcreateTime < input.EndTime.Value.AddDays(1).Date);

            var queryable = (from tquoteOrder in query
                             join tinquiryOrder in _dbContext.TinquiryOrder.WhereIf(() => input.InquiryStatus.HasValue, x => x.Fstatus == (int)input.InquiryStatus.Value)
                                 on new { tquoteOrder.ForderId, tquoteOrder.FinquiryOrderNo } equals new { tinquiryOrder.ForderId, tinquiryOrder.FinquiryOrderNo }
                             orderby tquoteOrder.FcreateTime descending,
                                 tquoteOrder.FquoteOrderNo descending
                             select new
                             {
                                 tquoteOrder.FquoteId,
                                 tquoteOrder.FquoteOrderNo,
                                 tquoteOrder.ForderId,
                                 tquoteOrder.FinquiryOrderNo,
                                 tinquiryOrder.FpackageCity,
                                 tinquiryOrder.FdeliveryCountry,
                                 tinquiryOrder.Fstatus,
                                 tquoteOrder.FuserId,
                                 tquoteOrder.FcompanyId,
                                 tquoteOrder.FcompanyName,
                                 tquoteOrder.Fcontent,
                                 tquoteOrder.Fremark,
                                 tquoteOrder.Fcancel,
                                 tquoteOrder.FcancelTime,
                                 tquoteOrder.FcancelReason,
                                 tquoteOrder.FcreateTime,
                                 tquoteOrder.Fviewed
                             }).ProjectTo<QuotePageDataOutput>();
            return queryable;
        }
    }
}