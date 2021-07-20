namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp.Dto
{
    /// <summary>
    /// 内部查询映射类
    /// </summary>
    internal class OrderDbQueryDto
    {
        public string DateFormat { get; set; }
        public decimal TotalCount { get; set; }

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
        public decimal AlipayCount { get; set; }
        public decimal WeixinCount { get; set; }

        public decimal UnknownStatusCount { get; set; }
        public decimal PendingCount { get; set; }
        public decimal SuccessCount { get; set; }
        public decimal DeliveringCount { get; set; }
        public decimal DeliveredCount { get; set; }
        public decimal ExpiredCount { get; set; }
        public decimal ClosedCount { get; set; }
        public decimal RefundingCount { get; set; }
        public decimal RefundedCount { get; set; }
        public decimal RefundFailureCount { get; set; }

    }
}