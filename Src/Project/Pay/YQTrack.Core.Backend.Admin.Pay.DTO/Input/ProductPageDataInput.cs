using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class ProductPageDataInput : PageInput
    {
        public long? ProductCategory { get; set; }

        public ServiceType[] ServiceType { get; set; }

        public UserRoleType[] Role { get; set; }

        public string Keyword { get; set; }
    }
}