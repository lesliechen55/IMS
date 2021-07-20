using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ProductPageDataResponse
    {
        public long ProductId { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string Role { get; set; }
        public string ServiceType { get; set; }
        public string SkuCount { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }

        public string IsSubscription { get; set; }
    }
}
