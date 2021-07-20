using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Core.Interceptor;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Freight.Service
{
    public interface IHomeService : IScopeService
    {
        [MemoryCacheInterceptor]
        Task<(int totalChannel, int totalInquiry, int totalQuote, int totalCarrier)> GetMainDataAsync();

        Task<(IEnumerable<ChannelPageDataOutput> outputs, int total)> GetChannelPageDataAsync(ChannelPageDataInput input);

        [OperationTrace(desc: "导出渠道数据", type: OperationType.Query)]
        Task<IEnumerable<ChannelPageDataOutput>> GetChannelExcelAsync(ChannelPageDataInput input);

        Task<(IEnumerable<QuotePageDataOutput> outputs, int total)> GetQuotePageDataAsync(QuotePageDataInput input);

        [OperationTrace(desc: "导出竞价单数据", type: OperationType.Query)]
        Task<IEnumerable<QuotePageDataOutput>> GetQuotePageInfoAsync(QuotePageDataInput input);
    }
}