namespace YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Request
{
    /// <summary>
    /// 店铺导入记录搜索条件
    /// </summary>
    public class TrackUploadRecordRequest
    {
        public long ShopId { get; set; }

        /// <summary>
        /// 用户路由
        /// </summary>
        public string UserRoute { set; get; }
    }
}
