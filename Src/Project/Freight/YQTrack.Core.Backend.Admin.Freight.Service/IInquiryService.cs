using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Freight.Service
{
    public interface IInquiryService : IScopeService
    {
        [OperationTrace(desc: "下架询价单", type: OperationType.Edit)]
        Task RejectInquiryAsync(int managerId, long orderId, string reason);

        Task<(IEnumerable<InquiryPageDataOutput> Outputs, int Total)> GetInquiryList(long? id, string title, string inquiryNo, byte? status, int page, int limit, string publisher, DateTime? publishStartTime, DateTime? publishEndTime, DateTime? expireStartTime, DateTime? expireEndTime);

        Task<(IEnumerable<InquiryOrderStatusLogPageDataOutput> Outputs, int Total)> GetInquiryOrderStatusLogPageDataAsync(InquiryOrderStatusLogPageDataInput input);
    }
}