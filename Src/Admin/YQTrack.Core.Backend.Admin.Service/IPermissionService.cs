using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.DTO.Input;
using YQTrack.Core.Backend.Admin.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Service
{
    public interface IPermissionService : IScopeService
    {
        Task<IEnumerable<PermissionOutput>> GetAllAsync();

        [OperationTrace(desc: "添加权限", type: OperationType.Add)]
        Task AddAsync(PermissionAddInput input, int createBy);

        [OperationTrace(desc: "修改权限信息", type: OperationType.Edit)]
        Task EditAsync(int id, string name, string areaName, string controllerName, string actionName, string fullName, string url, int? parentId, int sort, string remark, int updateBy, string icon, MenuType menuType, string topMenuKey, bool? isMultiAction);

        Task<PermissionOutput> GetByIdAsync(int id);
    }
}