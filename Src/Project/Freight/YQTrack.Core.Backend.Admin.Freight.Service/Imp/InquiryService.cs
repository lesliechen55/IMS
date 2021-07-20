using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Freight.Data;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;
using YQTrack.Core.Backend.Admin.Freight.Service.RemoteApi;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Freight.Service.Imp
{
    public class InquiryService : IInquiryService
    {
        private readonly CarrierContext _dbContext;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly FreightConfig _freightConfig;

        public InquiryService(CarrierContext dbContext,
                              IHttpClientFactory httpClientFactory,
                              IOptionsSnapshot<FreightConfig> freightOptions)
        {
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
            _freightConfig = freightOptions.Value;
        }

        public async Task RejectInquiryAsync(int managerId, long orderId, string reason)
        {
            var order = await _dbContext.TinquiryOrder.SingleOrDefaultAsync(x => x.ForderId == orderId);
            if (order == null || order.FdeleteFlag)
            {
                throw new BusinessException($"{nameof(orderId)}参数错误,数据不存在");
            }

            if (order.Fstatus == (byte)InquiryOrderStatus.MangerReject || order.Fstatus == (byte)InquiryOrderStatus.BusinessFailed)
            {
                throw new BusinessException($"错误:{order.FinquiryOrderNo}询价单状态已经是{((InquiryOrderStatus)order.Fstatus).GetDescription()},不能下架");
            }

            order.Fstatus = (byte)InquiryOrderStatus.MangerReject;
            order.FrejectReason = reason;
            order.FupdateTime = DateTime.UtcNow;
            order.FupdateBy = managerId;
            order.FstopSelfEnum = (int)StopInquiryOrderReasonEnum.ManagerReject;
            order.FprocessTime = DateTime.UtcNow;

            var rejectInquiryRequest = new RejectInquiryRequest()
            {
                Method = "RejectInquiryCallback",
                Version = "1.0",
                Param = new Dictionary<string, string>()
            };
            rejectInquiryRequest.Param.Add("id", orderId.ToString());
            rejectInquiryRequest.Param.Add("reason", reason);
            rejectInquiryRequest.Param.Add("managerId", managerId.ToString());
            var request = new HttpRequestMessage(HttpMethod.Post, _freightConfig.RemoteUrl)
            {
                Content = new StringContent(JsonConvert.SerializeObject(rejectInquiryRequest), Encoding.UTF8)
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Headers.Add("AuthKey", "ims");
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new BusinessException($"调用远程API失败,数据库修改失败,请重试");
            }

            var str = await response.Content.ReadAsStringAsync();

            var resultBase = JsonConvert.DeserializeObject<ResultBase>(str);

            if (resultBase.Code != 0)
            {
                throw new BusinessException(resultBase.Message);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<(IEnumerable<InquiryPageDataOutput> Outputs, int Total)> GetInquiryList(long? id, string title, string inquiryNo, byte? status, int page, int limit, string publisher, DateTime? publishStartTime, DateTime? publishEndTime, DateTime? expireStartTime, DateTime? expireEndTime)
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
            var count = await queryable.CountAsync();
            var outputs = await queryable.OrderByDescending(x => x.FcreateTime).ToPage(page, limit).ProjectTo<InquiryPageDataOutput>().ToListAsync();
            return (outputs, count);
        }

        public async Task<(IEnumerable<InquiryOrderStatusLogPageDataOutput> Outputs, int Total)> GetInquiryOrderStatusLogPageDataAsync(InquiryOrderStatusLogPageDataInput input)
        {
            var queryable = _dbContext.TInquiryOrderStatusLog
                .WhereIf(() => input.OrderId.HasValue && input.OrderId.Value > 0,
                    x => x.FOrderId == input.OrderId.Value)
                .WhereIf(() => input.StartTime.HasValue, x => x.FCreateTime >= input.StartTime.Value.Date)
                .WhereIf(() => input.EndTime.HasValue, x => x.FCreateTime < input.EndTime.Value.AddDays(1).Date);
            var count = await queryable.CountAsync();
            var outputs = await queryable.OrderByDescending(x => x.FCreateTime).ToPage(input.Page, input.Limit).ProjectTo<InquiryOrderStatusLogPageDataOutput>().ToListAsync();
            return (outputs, count);
        }
    }
}