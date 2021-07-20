namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp.Dto
{
    /// <summary>
    /// 内部查询映射类
    /// </summary>
    internal class PaymentDbQueryDto
    {
        public string DateFormat { get; set; }
        public decimal TotalCount { get; set; }

        public decimal UnknownProviderCount { get; set; }
        public decimal AlipayCount { get; set; }
        public decimal WeixinpayCount { get; set; }
        public decimal PaypalCount { get; set; }
        public decimal AppleiapCount { get; set; }
        public decimal GooglepayCount { get; set; }
        public decimal GlocashCount { get; set; }
        public decimal StripeCount { get; set; }
        public decimal ShopifyCount { get; set; }
        public decimal OfflinePayCount { get; set; }
        public decimal PresentCount { get; set; }
        public decimal AlipayMiniPayCount { get; set; }
        public decimal WeixinMiniPayCount { get; set; }

        public decimal UnknownServiceTypeCount { get; set; }
        public decimal DonateCount { get; set; }
        public decimal BuyerCount { get; set; }
        public decimal SellerCount { get; set; }
        public decimal ApiCount { get; set; }
        public decimal CarrierCount { get; set; }

        public decimal NoneCurrencyTypeCount { get; set; }
        public decimal CnyCount { get; set; }
        public decimal UsdCount { get; set; }

        public decimal UnknownPlatformTypeCount { get; set; }
        public decimal WebCount { get; set; }
        public decimal AndroidCount { get; set; }
        public decimal IosCount { get; set; }
        public decimal MobileCount { get; set; }
        public decimal AlipayPlatCount { get; set; }
        public decimal WeixinCount { get; set; }

        public decimal UnknownStatusCount { get; set; }
        public decimal PendingCount { get; set; }
        public decimal SuccessCount { get; set; }
        public decimal FailedCount { get; set; }
        public decimal CancelledCount { get; set; }
        public decimal RefundedCount { get; set; }

    }
}