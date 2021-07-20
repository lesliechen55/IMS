using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Entities;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    /// <summary>
    /// 优惠活动
    /// </summary>
    [Table("TActivity")]
    public class TActivity : Entity
    {
        /// <summary>
        /// 主键
        /// </summary>
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
        /// 使用描述
        /// </summary>
        public string FDescription { get; set; }

        /// <summary>
        /// <summary>活动类型</summary>
        /// </summary>
        public ActivityType FActivityType { get; set; }

        /// <summary>
        /// 活动优惠模式
        /// </summary>
        public CouponMode FCouponMode { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public BusinessCtrlType FBusinessType { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        public long FProductId { get; set; }

        /// <summary>
        /// 适用商品Sku集合
        /// </summary>
        public string FSkuCodes { get; set; }

        /// <summary>
        /// 适用商品Sku集合
        /// </summary>
        [NotMapped]
        public string[] SkuList
        {
            get
            {
                return FSkuCodes?.Replace("[", "").Replace("]", "").Replace("\"", "").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }

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
        /// 优惠规则
        /// </summary>
        public string FRules { get; set; }

        /// <summary>
        /// 活动规则
        /// </summary>
        [NotMapped]
        public IEnumerable<ActivityRule> Rules
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(FRules))
                {
                    return JsonConvert.DeserializeObject<List<ActivityRule>>(FRules);
                }
                return null;
            }
        }

        /// <summary>
        /// 有效期
        /// </summary>
        public int FTerm { get; set; }

        /// <summary>
        /// 是否内部使用
        /// </summary>
        public bool FInternalUse { get; set; }

        public virtual ICollection<TActivityCoupon> FActivityCoupons { get; set; }
    }
}
