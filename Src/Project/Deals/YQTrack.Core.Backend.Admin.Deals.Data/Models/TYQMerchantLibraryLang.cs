using System;

namespace YQTrack.Core.Backend.Admin.Deals.Data.Models
{
    public partial class TYQMerchantLibraryLang
    {
        public long FYQMerchantLibraryLangId { get; set; }
        public long? FYQMerchantLibraryId { get; set; }
        public string FName { get; set; }
        public string FAlsoName { get; set; }
        public string FLangCode { get; set; }
        public string FDescription { get; set; }
        public DateTime? FCreateDate { get; set; }
        public DateTime? FModifyDate { get; set; }
    }
}
