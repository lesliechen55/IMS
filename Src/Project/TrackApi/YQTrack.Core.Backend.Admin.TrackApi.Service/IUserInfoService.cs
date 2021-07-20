using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Input;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;

namespace YQTrack.Core.Backend.Admin.TrackApi.Service
{
    public interface IUserInfoService : IScopeService
    {
        //void SetApiTrack(string connectionString);
        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        /// <param name="input">用户分页列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<UserInfoPageDataOutput> outputs, int total)> GetPageDataAsync(UserInfoPageDataInput input);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserInfoOutput> GetByIdAsync(int id);

        /// <summary>
        /// 根据ID获取额度消耗实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserInfoViewOutput> GetViewDataByIdAsync(int id);

        /// <summary>
        /// 根据ID获取用户配置实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserConfigOutput> GetUserConfigByIdAsync(int id);

        /// <summary>
        /// 获取可用的最大的用户编号
        /// </summary>
        /// <returns></returns>
        Task<short> GetMaxUserNo();

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "注册API用户", type: OperationType.Add)]
        Task<bool> AddAsync(UserInfoEditInput input, int operatorId);

        /// <summary>
        /// 注册重试
        /// </summary>
        /// <param name="userNo"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "注册API用户重试", type: OperationType.Add)]
        Task<bool> ReregisterAsync(int userNo, int operatorId);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "修改API用户", type: OperationType.Edit)]
        Task<bool> EditAsync(UserInfoEditInput input, int operatorId);

        /// <summary>
        /// 修改API状态
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "修改API状态", type: OperationType.Edit)]
        Task ChangeStatusAsync(ChangeApiStateInput input, int operatorId);
    }
}
