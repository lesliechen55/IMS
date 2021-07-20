using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class ActivityEditInput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public long ActivityId { get; set; }

        /// <summary>
        /// 优惠活动中文名称
        /// </summary>
        public string CnName { get; set; }

        /// <summary>
        /// 优惠活动英文名称
        /// </summary>
        public string EnName { get; set; }

        /// <summary>
        /// 使用描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// <summary>活动类型</summary>
        /// </summary>
        public ActivityType ActivityType { get; set; }

        /// <summary>
        /// 活动优惠模式
        /// </summary>
        public CouponMode CouponMode { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessCtrlType BusinessType { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 适用商品Sku集合
        /// </summary>
        public string SkuCodes { get; set; }

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
        public ActivityStatus Status { get; set; }
        /// <summary>
        /// 优惠类型（额度优惠/金额优惠）
        /// </summary>
        public ActivityDiscountType DiscountType { get; set; }
        /// <summary>
        /// 优惠规则
        /// </summary>
        public string Rules { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public int Term { get; set; }
        /// <summary>
        /// 是否内部使用
        /// </summary>
        public bool FInternalUse { get; set; }

    }

    public class ActivityEditStatusInput {
        /// <summary>
        /// 主键
        /// </summary>
        public long ActivityId { get; set; }

        /// <summary>
        /// 活动状态
        /// </summary>
        public ActivityStatus Status { get; set; }
    }
}