namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response
{
    public class UserInvoiceResponse
    {
        public int InvoiceId { get; set; }
        public int TotalRequest { get; set; }
        public decimal FCNYAmount { get; set; }
        public decimal FUSDAmount { get; set; }
        public string Amount {
            get
            {
                string cny = FCNYAmount == 0 ? "" : $"CNY ￥{FCNYAmount}";
                string usd = FUSDAmount == 0 ? "" : string.IsNullOrWhiteSpace(cny) ? $"USD ${FUSDAmount}" : $" USD ${FUSDAmount}";
                return string.IsNullOrWhiteSpace(cny) && string.IsNullOrWhiteSpace(usd) ? "--" : (cny + usd);
            }
        }
    }
}
