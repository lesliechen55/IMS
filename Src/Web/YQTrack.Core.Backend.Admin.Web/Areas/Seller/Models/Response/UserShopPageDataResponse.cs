using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Response
{
    public class UserShopPageDataResponse
    {
        public string ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopAlias { get; set; }
        public string PlatformType { get; set; }
        /// <summary>
        /// 平台编号
        /// </summary>
        public string PlatformUID { get; set; }
        public string State { get; set; }
        public DateTime? LastSyncTime { get; set; }
        /// <summary>
        /// 最新同步数量
        /// </summary>
        public string LastSyncNum { get; set; }
        /// <summary>
        /// 下次同步时间
        /// </summary>
        public DateTime? NextSyncTime { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
