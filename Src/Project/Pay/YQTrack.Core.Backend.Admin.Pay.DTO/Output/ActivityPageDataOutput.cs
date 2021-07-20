using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class ActivityPageDataOutput
    {
        public long FActivityId { get; set; }

        /// <summary>
        /// 优惠活动中文名称
        /// </summary>
        public string FCnName { get; set; }

        /// <summary>
        /// 优惠活动英文名称
        /// </summary>
        public string FEnName { get; set; }

        /// <summary>
        /// <summary>活动类型</summary>
        /// </summary>
        public ActivityType FActivityType { get; set; }

        /// <summary>
        /// 优惠活动描述
        /// </summary>
        public string FDescription { get; set; }

        /// <summary>
        /// 活动优惠模式
        /// </summary>
        public CouponMode FCouponMode { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessCtrlType FBusinessType { get; set; }



        /// <summary>
        /// 商品ID
        /// </summary>
        public string FProductId  { get; set; }

        /// <summary>
        /// 商品Sku集合
        /// </summary>
        public string FSkuCodes { get; set; }


        /// <summary>
        /// 使用规则
        /// </summary>
        public string FRules { get; set; }


        /// <summary>
        /// 有效期
        /// </summary>
        public string FTerm { get; set; }




        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime FStartTime { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime FEndTime { get; set; }

        /// <summary>
        /// 活动状态
        /// </summary>
        public ActivityStatus FStatus { get; set; }

        /// <summary>
        /// 优惠类型（额度优惠/金额优惠）
        /// </summary>
        public ActivityDiscountType FDiscountType { get; set; }

        /// <summary>
        /// 是否内部使用
        /// </summary>
        public bool FInternalUse { get; set; }
    }
}
