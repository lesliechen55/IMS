using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Freight.Service
{
    public interface IReportService : IScopeService
    {
        Task<(IEnumerable<ReportPageDataOutput> Outputs, int Total)> GetReportPageDataAsync(ReportPageDataInput input);

        Task<(short status, string desc)> GetStatusAsync(long id);

        [OperationTrace(desc: "处理渠道举报数据", type: OperationType.Edit)]
        Task ProcessAsync(long id, ProcessReportStatusEnum status, string remark, string detail);
    }
}