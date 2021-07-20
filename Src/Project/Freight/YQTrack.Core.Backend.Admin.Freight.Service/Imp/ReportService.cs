using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Freight.Data;
using YQTrack.Core.Backend.Admin.Freight.Data.Models;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;
using YQTrack.Core.Backend.Admin.Freight.Service.RemoteApi;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Freight.Service.Imp
{
    public class ReportService : IReportService
    {
        private readonly CarrierContext _carrierContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptionsSnapshot<FreightConfig> _freightOptions;

        public ReportService(CarrierContext carrierContext,
                             IHttpClientFactory httpClientFactory,
                             IOptionsSnapshot<FreightConfig> freightOptions)
        {
            _carrierContext = carrierContext;
            _httpClientFactory = httpClientFactory;
            _freightOptions = freightOptions;
        }

        public async Task<(IEnumerable<ReportPageDataOutput> Outputs, int Total)> GetReportPageDataAsync(ReportPageDataInput input)
        {
            var queryable = from tchannelReportRecord in _carrierContext.TchannelReportRecord.WhereIf(() => input.ProcessReportStatus.HasValue, x => x.FprocessStatus == (byte)input.ProcessReportStatus.Value)
                            join tchannel in _carrierContext.Tchannel.WhereIf(() => !input.ChannelName.IsNullOrWhiteSpace(), x => x.FchannelTitle.Contains(input.ChannelName.Trim())) on tchannelReportRecord.FchannelId equals tchannel.FchannelId
                            join tcompany in _carrierContext.Tcompany.WhereIf(() => !input.CompanyName.IsNullOrWhiteSpace(), x => x.FcompanyName.Contains(input.CompanyName.Trim())) on tchannelReportRecord.FcompanyId equals tcompany.FcompanyId
                            select new
                            {
                                tchannelReportRecord.Fid,
                                tchannel.FchannelTitle,
                                tcompany.FcompanyName,
                                tchannelReportRecord.FreasonType,
                                tchannelReportRecord.Fdetail,
                                tchannelReportRecord.FreportEmail,
                                tchannelReportRecord.FreportTime,
                                tchannelReportRecord.FprocessStatus,
                                tchannelReportRecord.FprocessTime,
                                tchannelReportRecord.FprocessRemark,
                                tchannelReportRecord.FprocessDescription
                            };

            var total = await queryable.CountAsync();

            var outputs = await queryable.OrderByDescending(x => x.FprocessTime).ThenByDescending(x => x.FreportTime).ProjectTo<ReportPageDataOutput>().ToPage(input.Page, input.Limit).ToListAsync();

            return (outputs, total);
        }

        public async Task<(short status, string desc)> GetStatusAsync(long id)
        {
            var record = await GetAsync(id);
            return (record.FprocessStatus, record.FprocessDescription);
        }

        private async Task<TchannelReportRecord> GetAsync(long id)
        {
            var record = await _carrierContext.TchannelReportRecord.SingleOrDefaultAsync(x => x.Fid == id);
            if (record == null)
            {
                throw new BusinessException($"参数错误:{nameof(id)}:{id},找不到举报数据");
            }
            return record;
        }

        private async Task<Tchannel> GetChannelAsync(long id)
        {
            var channel = await _carrierContext.Tchannel.SingleOrDefaultAsync(x => x.FchannelId == id);
            if (channel == null)
            {
                throw new BusinessException($"参数错误:{nameof(id)}:{id},找不到渠道数据");
            }
            return channel;
        }

        private async Task<Tcompany> GetCompanyAsync(long id)
        {
            var company = await _carrierContext.Tcompany.SingleOrDefaultAsync(x => x.FcompanyId == id);
            if (company == null)
            {
                throw new BusinessException($"参数错误:{nameof(id)}:{id},找不到公司数据");
            }
            return company;
        }

        public async Task ProcessAsync(long id, ProcessReportStatusEnum status, string remark, string detail)
        {
            var record = await GetAsync(id);
            var channel = await GetChannelAsync(record.FchannelId);
            var company = await GetCompanyAsync(record.FcompanyId);

            // 处理为有效举报并且数据库该记录状态之前不是有效举报
            if (status == ProcessReportStatusEnum.ValidReport && record.FprocessStatus != (short)ProcessReportStatusEnum.ValidReport)
            {
                channel.FvalidReportTimes = channel.FvalidReportTimes + 1;
                company.FchannelValidReportTimes = company.FchannelValidReportTimes + 1;
            }

            // 有效举报处理为无效举报
            if (status == ProcessReportStatusEnum.InvalidReport &&
                record.FprocessStatus == (short)ProcessReportStatusEnum.ValidReport)
            {
                channel.FvalidReportTimes = channel.FvalidReportTimes - 1;
                company.FchannelValidReportTimes = company.FchannelValidReportTimes - 1;
            }

            // 触发邮件逻辑(处理为有效举报并且数据库该记录状态之前不是有效举报)
            if (status == ProcessReportStatusEnum.ValidReport &&
                record.FprocessStatus != (short)ProcessReportStatusEnum.ValidReport)
            {
                await CallRemoteApiAsync(new Dictionary<string, string>
                {
                    {"channelName",channel.FchannelTitle },
                    {"reportUserId",record.FreportUserId.ToString() },
                    {"reportUserRole",record.FreportUserRole.ToString() },
                    {"reportEmail",record.FreportEmail },
                    {"reportUserNickName",record.FreportUserNickName },
                    {"reportUserLanguage",record.FreportUserLanguage }
                });
            }

            remark = string.Format($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} : {remark}");
            string remarkCombine = string.Format($"{remark} </br> {record.FprocessRemark ?? string.Empty}");

            record.FprocessStatus = (byte)status;
            record.FprocessTime = DateTime.UtcNow;
            record.FprocessRemark = remarkCombine;
            record.FupdateTime = DateTime.UtcNow;

            // 有效举报记录处理详情说明
            if (status == ProcessReportStatusEnum.ValidReport)
            {
                record.FprocessDescription = detail;
            }

            await _carrierContext.SaveChangesAsync();
        }


        private async Task CallRemoteApiAsync(Dictionary<string, string> param)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_freightOptions.Value.CompanyAdminRemoteUrl), "/Login/ProcessReport"))
            {
                Content = new FormUrlEncodedContent(param)
            };
            request.Headers.Add("AuthKey", "ims");
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new BusinessException($"调用远程API失败,数据库修改失败,请重试,{response.ReasonPhrase}");
            }
            var str = await response.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<ApiResult>(str);
            if (!apiResult.Success)
            {
                throw new BusinessException(apiResult.Msg);
            }
        }
    }
}