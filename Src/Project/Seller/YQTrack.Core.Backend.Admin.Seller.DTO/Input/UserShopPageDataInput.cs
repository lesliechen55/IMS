
using YQTrack.Core.Backend.Admin.User.DTO;
using YQTrack.Core.Backend.Enums.Seller;

namespace YQTrack.Core.Backend.Admin.Seller.DTO.Input
{
    /// <summary>
    /// 用户店铺列表搜索条件
    /// </summary>
    public class UserShopPageDataInput : PageInput
    {
        public string FShopName { get; set; }
        public int? FPlatformType { get; set; }
        public ShopStateType? FState { get; set; }

        /// <summary>
        /// 用户路由
        /// </summary>
        public UserRouteDto UserRoute { set; get; }
    }
}
