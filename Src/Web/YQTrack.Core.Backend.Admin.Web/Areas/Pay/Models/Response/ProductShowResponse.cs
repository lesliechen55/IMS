using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ProductShowResponse
    {
        public long ProductCategoryId { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public UserRoleType Role { get; set; }
        public ServiceType ServiceType { get; set; }
        public long CreateBy { get; set; }
        public DateTime CreateAt { get; set; }
        public long UpdateBy { get; set; }
        public DateTime UpdateAt { get; set; }
        public ProductSelectDataResponse ProductSelectData { get; set; }

        public bool IsSubscription { get; set; }
    }
}
