namespace YQTrack.Core.Backend.Admin.TrackApi.DTO.Output
{
    public class UserInvoiceOutput
    {
        public int FInvoiceId { get; set; }
        public int FTotalRequest { get; set; }
        public decimal FCNYAmount { get; set; }
        public decimal FUSDAmount { get; set; }
    }
}
