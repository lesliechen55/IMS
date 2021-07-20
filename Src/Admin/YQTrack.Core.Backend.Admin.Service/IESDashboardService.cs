using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.DTO;
using YQTrack.Core.Backend.Admin.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Service
{
    public interface IESDashboardService : IScopeService
    {
        Task<ESDashboardDetailOutput> GetByIdAsync(int id);

        [OperationTracePlus(desc: "配置Dashboard", type: OperationType.Edit)]
        Task SetAsync(ESDashboardDto input);

        Task<IEnumerable<ESFieldOutput>> GetDataByCategoryAsync(string category);

        Task<ESDashboardDetailOutput> GetByPermissionCodeAsync(string permissionCode);
    }
}