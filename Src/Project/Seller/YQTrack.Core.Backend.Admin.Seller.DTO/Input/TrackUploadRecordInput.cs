
using YQTrack.Core.Backend.Admin.User.DTO;

namespace YQTrack.Core.Backend.Admin.Seller.DTO.Input
{
    /// <summary>
    /// 店铺导入记录搜索条件
    /// </summary>
    public class TrackUploadRecordInput
    {
        public long FShopId { get; set; }

        /// <summary>
        /// 用户路由
        /// </summary>
        public UserRouteDto UserRoute { set; get; }
    }
}
