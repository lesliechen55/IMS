using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class ProductSkuAddPriceInput
    {
        public long FProductSkuId { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public string FDescription { get; set; }
        public UserPlatformType FPlatformType { get; set; }
        public decimal FSaleUnitPrice { get; set; }
        public decimal FAmount { get; set; }
    }
}