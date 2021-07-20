using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.User.DTO.Input;
using YQTrack.Core.Backend.Admin.User.DTO.Output;

namespace YQTrack.Core.Backend.Admin.User.Service
{
    public interface IUserService : IScopeService
    {
        Task<(IEnumerable<UserPageDataOutput> outputs, int total)> GetUserPageDataAsync(UserPageDataInput input);

        Task<(IEnumerable<UserFeedbackPageDataOutput> outputs, int total)> GetUserFeedbackPageDataAsync(
            UserFeedbackPageDataInput input);

        [OperationTracePlus("回复反馈意见", OperationType.Edit)]
        Task Reply(string feedbackIds, string content, int updateBy);

        Task<UserDetailOutput> GetDetailAsync(long userId);

        /// <summary>
        /// 根据ID获取用户基本信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserBaseInfoOutput> GetUserBaseInfoAsync(long userId);

        /// <summary>
        /// 根据邮件获取用户基本信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UserBaseInfoOutput> GetUserBaseInfoByEmailAsync(string email);

        /// <summary>
        /// 获取用户数据库路由信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserDataRouteOutput> GetUserDataRouteAsync(long userId);


        /// <summary>
        /// 获取用户数据库路由信息集合
        /// </summary>
        /// <param name="userIdList"></param>
        /// <returns></returns>
        Task<IEnumerable<UserDataRoutePlusOutput>> GetUserDataRouteListAsync(long[] userIdList);

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [OperationTracePlus(desc: "注销用户", type: OperationType.Delete)]
        Task<(bool success, string message)> DeleteUserAsync(long userId, string email);
    }
}