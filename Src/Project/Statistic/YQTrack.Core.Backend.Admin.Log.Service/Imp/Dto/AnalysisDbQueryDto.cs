namespace YQTrack.Core.Backend.Admin.Log.Service.Imp.Dto
{
    public class AnalysisDbQueryDto
    {
        public string DateFormat { get; set; }

        public int NoneCount { get; set; }
        public int SellerCount { get; set; }
        public int BuyerCount { get; set; }
        public int CarrierCount { get; set; }
        public int GuestCount { get; set; }

        public int AliexpressCount { get; set; }
        public int EbayCount { get; set; }
        public int DHgateCount { get; set; }
        public int WishCount { get; set; }

        public int NoneDeviceTypeCount { get; set; }
        public int IPhoneCount { get; set; }
        public int IPadCount { get; set; }
        public int AndroidCount { get; set; }
        public int AndroidPadCount { get; set; }
        public int WPPhoneCount { get; set; }
        public int WPPCCount { get; set; }

    }
}