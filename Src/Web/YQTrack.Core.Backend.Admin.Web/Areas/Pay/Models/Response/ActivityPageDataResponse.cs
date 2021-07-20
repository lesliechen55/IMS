using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ActivityPageDataResponse
    {
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
        /// 优惠活动描述
        /// </summary>
        public string FDescription { get; set; }

        /// <summary>
        /// <summary>活动类型</summary>
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        /// 活动优惠模式
        /// </summary>
        public string CouponMode { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 商品Sku集合
        /// </summary>
        public string SkuCodes { get; set; }

        /// <summary>
        /// 使用规则
        /// </summary>
        public string Rules { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public int Term { get; set; }

        /// <summary>
        /// 活动开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 活动结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 活动状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 优惠类型（额度优惠/金额优惠）
        /// </summary>
        public string DiscountType { get; set; }

        /// <summary>
        /// 是否内部使用
        /// </summary>
        public bool InternalUse { get; set; }
    }

}
