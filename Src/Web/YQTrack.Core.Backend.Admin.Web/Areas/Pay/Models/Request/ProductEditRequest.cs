using System.ComponentModel;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ProductEditRequest
    {
        public string ProductCategoryId { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        //public bool Active { get; set; }
        public UserRoleType Role { get; set; }
        public ServiceType ServiceType { get; set; }

        [DisplayName("是否订阅")]
        public bool? IsSubscription { get; set; }
    }
}
