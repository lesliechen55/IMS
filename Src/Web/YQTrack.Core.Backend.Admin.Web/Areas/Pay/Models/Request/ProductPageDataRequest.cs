using System.ComponentModel;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ProductPageDataRequest : PageRequest
    {
        [DisplayName("商品分类")]
        public long? ProductCategory { get; set; }

        [DisplayName("服务类型")]
        public ServiceType[] ServiceType { get; set; }

        [DisplayName("适用角色")]
        public UserRoleType[] Role { get; set; }

        [DisplayName("名称/编码/描述")]
        public string Keyword { get; set; }
    }
}