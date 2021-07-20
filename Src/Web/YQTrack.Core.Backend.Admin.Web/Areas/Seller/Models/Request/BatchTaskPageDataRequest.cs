using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Seller;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Request
{
    /// <summary>
    /// 大批量任务列表搜索条件
    /// </summary>
    public class BatchTaskPageDataRequest : PageRequest
    {

        /// <summary>
        ///  任务类型
        /// </summary>
        public int? TaskType { set; get; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TrackBatchTaskStatus? TaskStatus { set; get; }

        /// <summary>
        /// 用户路由
        /// </summary>
        public string UserRoute { set; get; }
    }
}
