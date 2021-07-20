using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class ProductEditInput
    {
        public long ProductCategoryId { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        //public bool Active { get; set; }
        public UserRoleType Role { get; set; }
        public ServiceType ServiceType { get; set; }

        public bool IsSubscription { get; set; }
    }
}
