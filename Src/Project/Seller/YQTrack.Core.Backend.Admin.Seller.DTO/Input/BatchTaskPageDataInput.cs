
using YQTrack.Core.Backend.Admin.User.DTO;
using YQTrack.Core.Backend.Enums.Seller;

namespace YQTrack.Core.Backend.Admin.Seller.DTO.Input
{
    /// <summary>
    /// 大批量任务列表搜索条件
    /// </summary>
    public class BatchTaskPageDataInput : PageInput
    {
        /// <summary>
        ///  任务类型
        /// </summary>
        public int? FTaskType { set; get; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TrackBatchTaskStatus? FTaskStatus { set; get; }

        /// <summary>
        /// 用户路由
        /// </summary>
        public UserRouteDto UserRoute { set; get; }
    }
}
