using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ActivityEditRequest
    {
        public long? ActivityId { get; set; }
        public string CnName { get; set; }
        public string EnName { get; set; }
        public string Description { get; set; }
        public ActivityType ActivityType { get; set; }
        public ActivityDiscountType DiscountType { get; set; }
        public CouponMode CouponMode { get; set; }
        public BusinessCtrlType BusinessType { get; set; }
        public long ProductId { get; set; }
        public ICollection<string> SkuCodes { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Term { get; set; }
        public int? Days { get; set; }
        public bool InternalUse { get; set; }
        public ICollection<Rule> Rules { get; set; }
        public ActivityEditRequest()
        {
            Rules = new List<Rule>();
        }
    }

    public class Rule
    {
        //[JsonProperty("id")]
        //public int Id { get; set; }
        //[JsonProperty("c")]
        //public CurrencyType Currency { get; set; }
        //[JsonProperty("t")]
        //public int Threshold { get; set; }
        //[JsonProperty("d")]
        //public int Discount { get; set; }

       
        public int id { get; set; }
     
         /// <summary>
         /// 货币
         /// </summary>
        public int c { get; set; }

        /// <summary>
        /// 阈值
        /// </summary>
        public int t { get; set; }

       /// <summary>
       /// 优惠
       /// </summary>
        public int d { get; set; }

    }
}
