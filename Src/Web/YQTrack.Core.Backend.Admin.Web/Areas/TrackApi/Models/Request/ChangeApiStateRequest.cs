using YQTrack.Core.Backend.Enums.TrackApi;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request
{
    public class ChangeApiStateRequest
    {
        public short UserNo { get; set; }
        public ApiState ApiState { get; set; }
    }
}
