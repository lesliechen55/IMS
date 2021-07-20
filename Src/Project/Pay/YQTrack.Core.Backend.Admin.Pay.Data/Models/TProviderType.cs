using System;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TProviderType
    {
        public long FProviderTypeId { get; set; }
        public string FName { get; set; }
        public string FCode { get; set; }
        public string FDescription { get; set; }
        public bool FActive { get; set; }
        public int FPlatformType { get; set; }
        public bool FMultiCurrency { get; set; }
        public bool FMultiSku { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }
    }
}
