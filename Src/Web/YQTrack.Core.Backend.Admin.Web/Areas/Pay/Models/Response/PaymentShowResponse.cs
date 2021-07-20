using System;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class PaymentShowResponse
    {
        public long OrderId { get; set; }
        public string PlatformType { get; set; }
        public string CurrencyType { get; set; }
        public string ProviderId { get; set; }
        public string ServiceType { get; set; }
        public string OrderName { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string ProviderTradeNo { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public List<PaymentLogResponse> PaymentLog { get; set; }
    }

    public class PaymentLogResponse
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        ///// <summary>
        ///// 请求内容
        ///// </summary>
        //public string Request { get; set; }
        ///// <summary>
        ///// 响应内容
        ///// </summary>
        //public string Response { get; set; }
        /// <summary>
        /// 客户端地址
        /// </summary>
        public string ClientIP { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
