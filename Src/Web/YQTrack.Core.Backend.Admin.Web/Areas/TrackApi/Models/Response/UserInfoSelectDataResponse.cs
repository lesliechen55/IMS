using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Enums.TrackApi;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response
{
    public class UserInfoSelectDataResponse
    {
        /// <summary>
        /// API状态
        /// </summary>
        public Dictionary<int, string> ListApiState => EnumHelper.GetSelectItem<ApiState>();
    }
}
