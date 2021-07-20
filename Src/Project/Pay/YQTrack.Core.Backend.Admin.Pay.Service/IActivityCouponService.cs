using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IActivityCouponService : IScopeService
    {
        /// <summary>
        /// 获取优惠券分页列表
        /// </summary>
        /// <param name="input">商品列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<ActivityCouponPageDataOutput> outputs, int total)> GetPageDataAsync(ActivityCouponPageDataInput input);

        /// <summary>
        /// 发放优惠券
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "发放优惠券", type: OperationType.Add)]
        Task<int> AddActivityCouponAsync(ActivityCouponEditInput input, int operatorId);
    }
}
