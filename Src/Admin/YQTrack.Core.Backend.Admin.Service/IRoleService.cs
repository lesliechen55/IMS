using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.DTO.Input;
using YQTrack.Core.Backend.Admin.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Service
{
    public interface IRoleService : IScopeService
    {
        Task<(IEnumerable<RolePageDataOutput> Outputs, int Total)> GetPageDataAsync(string name, int page, int size);

        [OperationTrace("添加角色", OperationType.Add)]
        Task AddAsync(RoleAddInput input);

        [OperationTrace(desc: "修改角色信息", type: OperationType.Edit)]
        Task EditAsync(int id, string name, bool isActive, string remark, int updateBy);

        Task<IEnumerable<RolePermissionOutput>> GetRolePermissionListAsync(int id);

        [OperationTrace(desc: "设置角色权限", type: OperationType.Edit)]
        Task SetPermissionListAsync(int roleId, int[] permissionIdList);

        Task<(RolePageDataOutput output, string[] roleUserNameList)> GetByIdAsync(int id);

        [OperationTracePlus("删除角色", OperationType.Delete)]
        Task DeleteAsync(int id);
    }
}