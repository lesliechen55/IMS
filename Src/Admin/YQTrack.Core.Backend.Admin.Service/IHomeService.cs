using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Interceptor;
using YQTrack.Core.Backend.Admin.DTO.Input;
using YQTrack.Core.Backend.Admin.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Service
{
    public interface IHomeService : IScopeService
    {
        Task<(int UserId, string Account, string NickName)> LoginAsync(string account, string pwd, string ip, string userAgent, string platForm = "web", bool isLog = true);

        [MemoryCacheInterceptor]
        IEnumerable<string> GetExistPermissionList(int managerId);

        [MemoryCacheInterceptor]
        Task<(int totalUser, int totalRole, int totalPermission, DateTime lastLoginTime)> GetMainDataAsync(int loginId);

        [MemoryCacheInterceptor]
        Task<Dictionary<string, MenuOutput[]>> GetMenuAsync(int managerId);

        [MemoryCacheInterceptor]
        Task<(Dictionary<string, string> TopKeyAndNameDic, string defaultSelectedTopKey, string Avatar)> GetTopKeyAndNameDicAsync(int managerId);

        Task<(IEnumerable<LoginLogPageDataOutput> outputs, int total)> GetLoginLogPageDataAsync(LoginLogPageDataInput input);

        Task<(IEnumerable<OperationLogPageDataOutput> outputs, int total)> GetOperationLogPageDataAsync(
            OperationLogPageDataInput input);

        Task<bool> HasSuperRoleAsync(int managerId);
    }
}