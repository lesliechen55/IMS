using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.DTO;
using YQTrack.Core.Backend.Enums.Seller;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Response
{
    public class BatchTaskSelectDataResponse
    {
        /// <summary>
        /// 大批量任务类型列表
        /// </summary>
        public Dictionary<int, string> BatchTaskTypeList => LanguageHelper.GetBatchTaskTypeList();

        /// <summary>
        /// 大批量任务状态列表
        /// </summary>
        public Dictionary<int, string> BatchTaskStateList => EnumHelper.GetSelectItem<TrackBatchTaskStatus>();

        /// <summary>
        /// 用户路由
        /// </summary>
        public string UserRoute { set; get; }

        /// <summary>
        /// 用户路由
        /// </summary>
        public UserRouteDto UserRouteDto { set; get; }
    }
}
