using System;
using YQTrack.Core.Backend.Enums.Seller;

namespace YQTrack.Core.Backend.Admin.Seller.DTO.Output
{
    public class UserShopPageDataOutput
    {
        public long FShopId { get; set; }
        public string FShopName { get; set; }
        public string FShopAlias { get; set; }
        public int FPlatformType { get; set; }
        /// <summary>
        /// 平台编号
        /// </summary>
        public string FPlatformUID { get; set; }
        public ShopStateType? FState { get; set; }
        public DateTime? FLastSyncTime { get; set; }
        /// <summary>
        /// 最新同步数量
        /// </summary>
        public int FLastSyncNum { get; set; }
        /// <summary>
        /// 下次同步时间
        /// </summary>
        public DateTime? FNextSyncTime { get; set; }
        public DateTime FCreateTime { get; set; }
    }
}
