using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class OfflineProductSkuPriceOutput
    {
        public long FProductSkuId { get; set; }
        public string FCode { get; set; }
        public string FName { get; set; }
        public ServiceType FServiceType { get; set; }
        public UserMemberLevel FMemberLevel { get; set; }
        public string FBusiness { get; set; }
        public UserPlatformType FPlatformType { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public decimal FSaleUnitPrice { get; set; }
        public decimal FAmount { get; set; }
    }
}