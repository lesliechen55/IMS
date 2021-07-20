using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.Data.Models;

namespace YQTrack.Core.Backend.Admin.CommonService
{
    public interface IUserInfoService : IScopeService
    {
        /// <summary>
        /// 通过邮箱获取用户ID(Email参数错误会导致异常)
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<long?> GetUserIdByEmailAsync(string email);

        /// <summary>
        /// 批量获取用户的邮箱
        /// </summary>
        /// <param name="userIdList"></param>
        /// <returns></returns>
        Task<Dictionary<long, string>> GetEmailListByUserIdListAsync(long[] userIdList);

        Task<TuserInfo> GetRequiredByIdAsync(long userId);

        //获取用户名称
        Task<Dictionary<long, string>> GetUserNickName(long[] userIdList);
    }
}
