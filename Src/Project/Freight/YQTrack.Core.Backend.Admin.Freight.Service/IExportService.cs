using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Freight.Service
{
    public interface IExportService : IScopeService
    {
        [OperationTrace(desc: "导出运输商信息", type: OperationType.Query)]
        Task<IEnumerable<ExportCarrierOutput>> GetCarrierInfoAsync();

        [OperationTrace(desc: "导出有效渠道信息", type: OperationType.Query)]
        Task<IEnumerable<ExportValidChannelOutput>> GetChannelInfoAsync();

        [OperationTrace(desc: "导出无效渠道信息", type: OperationType.Query)]
        Task<IEnumerable<ExportInvalidChannelOutput>> GetInvalidChannelInfoAsync();

        [OperationTrace(desc: "导出询价单信息", type: OperationType.Query)]
        Task<IEnumerable<InquiryPageDataOutput>> GetInquiryInfoAsync(long? id, string title, string inquiryNo, byte? status, string publisher, DateTime? publishStartTime, DateTime? publishEndTime, DateTime? expireStartTime, DateTime? expireEndTime);
    }
}