using System;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class ProductShowOutput
    {
        public long FProductCategoryId { get; set; }
        public long FProductId { get; set; }
        public string FName { get; set; }
        public string FCode { get; set; }
        public string FDescription { get; set; }
        public bool FActive { get; set; }
        public string Role { get; set; }
        public string ServiceType { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }

        public bool FIsSubscription { get; set; }
    }
}
