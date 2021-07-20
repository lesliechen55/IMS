using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.DTO;
using YQTrack.Core.Backend.Enums.Seller;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Response
{
    public class UserShopSelectDataResponse
    {
        /// <summary>
        /// 店铺状态
        /// </summary>
        public Dictionary<int, string> ShopStateList => EnumHelper.GetSelectItem<ShopStateType>();

        /// <summary>
        /// 店铺平台类型列表
        /// </summary>
        public Dictionary<int, string> PlatTypeList => LanguageHelper.GetPlatformList();

        /// <summary>
        /// 用户路由
        /// </summary>
        public string UserRoute { get; set; }

        /// <summary>
        /// 用户路由
        /// </summary>
        public UserRouteDto UserRouteDto { set; get; }
    }
}
