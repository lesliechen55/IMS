using System;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TProductCategory
    {
        public TProductCategory()
        {
            TProduct = new HashSet<TProduct>();
        }

        public long FProductCategoryId { get; set; }
        public string FName { get; set; }
        public string FCode { get; set; }
        public string FDescription { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }

        public ICollection<TProduct> TProduct { get; set; }
    }
}