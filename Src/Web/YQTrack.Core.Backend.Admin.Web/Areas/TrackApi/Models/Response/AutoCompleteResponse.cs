using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response
{
    public class AutoCompleteResponse
    {
        public int Code { get; set; } = 0;
        public string Type { get; set; } = "success";
        public IEnumerable<AutoCompleteItemResponse> Content { get; set; } = new List<AutoCompleteItemResponse>();
    }

    public class AutoCompleteItemResponse
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}