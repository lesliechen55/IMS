using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Seller;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Request
{
    /// <summary>
    /// 用户店铺列表搜索条件
    /// </summary>
    public class UserShopPageDataRequest : PageRequest
    {
        public string ShopName { get; set; }
        public int? PlatformType { get; set; }
        public ShopStateType? State { get; set; }

        /// <summary>
        /// 用户路由
        /// </summary>
        public string UserRoute { set; get; }
    }
}
