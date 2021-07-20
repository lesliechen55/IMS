using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.DTO.Input;
using YQTrack.Core.Backend.Admin.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Service
{
    public interface IManagerService : IScopeService
    {
        Task<(IEnumerable<ManagerPageDataOutput> Outputs, int Total)> GetPageDataAsync(ManagerPageDataInput input);

        [OperationTrace(desc: "添加管理员", type: OperationType.Add)]
        Task AddAsync(ManagerAddInput input, int createBy);

        [OperationTrace(desc: "修改管理员信息", type: OperationType.Edit)]
        Task EditAsync(int id, string nickName, string pwd, bool isLock, int updateBy, string remark);

        Task<IEnumerable<ManagerRoleOutput>> GetRoleListAsync(int id);

        [OperationTrace(desc: "设置管理员角色", type: OperationType.Edit)]
        Task SetRoleListAsync(int userId, int[] roleIdList);

        Task<ManagerPageDataOutput> GetByIdAsync(int id);

        [OperationTrace(desc: "修改密码", type: OperationType.Edit)]
        Task ChangePwdAsync(int id, string newPwd, string oldPwd);

        [OperationTrace(desc: "修改昵称", type: OperationType.Edit)]
        Task UpdateNickNameAsync(int id, string nickName);

        [OperationTrace(desc: "修改头像", type: OperationType.Edit)]
        Task UpdateAvatarAsync(int id, string path);

        [OperationTracePlus("强制删除用户", OperationType.Delete)]
        Task DeleteAsync(int id);
    }
}