using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class ProductPageDataOutput
    {
        public long FProductId { get; set; }
        public string FName { get; set; }
        public string CategoryName { get; set; }
        public string FCode { get; set; }
        public string FDescription { get; set; }
        public bool FActive { get; set; }
        public UserRoleType FRole { get; set; }
        public ServiceType FServiceType { get; set; }
        public int SkuCount { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }

        public bool FIsSubscription { get; set; }
    }
}
