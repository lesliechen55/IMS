using System;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TProvider
    {
        public int FProviderId { get; set; }
        public long FProviderTypeId { get; set; }
        public string FName { get; set; }
        public string FDescription { get; set; }
        public byte? FCurrencyType { get; set; }
        public bool FActive { get; set; }
        public int FPriority { get; set; }
        public bool FSandbox { get; set; }
        public string FIconUrl { get; set; }
        public string FPrefix { get; set; }
        public string FSetting { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }
    }
}
