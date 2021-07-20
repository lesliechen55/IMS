using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IActivityService : IScopeService
    {
        /// <summary>
        /// 获取商品分页列表
        /// </summary>
        /// <param name="input">商品列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<ActivityPageDataOutput> outputs, int total)> GetPageDataAsync(ActivityPageDataInput input);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ActivityPageDataOutput> GetByIdAsync(int id);

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "新增活动", type: OperationType.Add)]
        Task<bool> AddActivityAsync(ActivityEditInput input, int operatorId);

        /// <summary>
        /// 修改优惠活动
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "修改优惠活动信息", type: OperationType.Edit)]
        Task<bool> EditAsync(ActivityEditInput input, int operatorId);

        /// <summary>
        /// 修改优惠活动状态
        /// </summary>
        /// <param name="productId">活动ID</param>
        /// <param name="active">状态</param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "修改优惠活动状态", type: OperationType.Edit)]
        Task<bool> ChangeStatusAsync(ActivityEditStatusInput input, int operatorId);
    }
}
