namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ProductSkuBusinessResponse
    {
        public string BusinessCtrlType { get; set; }
        public string ConsumeType { get; set; }
        public bool Renew { get; set; }
        public byte Validity { get; set; }
        public int Quantity { get; set; }
    }
}