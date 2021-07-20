using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class PaymentShowOutput
    {
        public long FOrderId { get; set; }
        public UserPlatformType FPlatformType { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public PaymentProvider FProviderId { get; set; }
        public ServiceType FServiceType { get; set; }
        public string FOrderName { get; set; }
        public decimal? FPaymentAmount { get; set; }
        public string FProviderTradeNo { get; set; }
        public PaymentStatus FPaymentStatus { get; set; }
        public DateTime FCreateAt { get; set; }
        public DateTime FUpdateAt { get; set; }
        public List<PaymentLogOutput> PaymentLog { get; set; }
    }
    public class PaymentLogOutput
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public long FUserId { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public string FAction { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool FSuccess { get; set; }
        ///// <summary>
        ///// 请求内容
        ///// </summary>
        //public string FRequest { get; set; }
        ///// <summary>
        ///// 响应内容
        ///// </summary>
        //public string FResponse { get; set; }
        /// <summary>
        /// 客户端地址
        /// </summary>
        public string FClientIP { get; set; }
        public DateTime FCreateAt { get; set; }
    }
}
