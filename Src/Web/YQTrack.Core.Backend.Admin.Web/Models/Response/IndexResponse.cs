using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Web.Models.Response
{
    public class IndexResponse
    {
        public Dictionary<string, string> TopKeyAndNameDic { get; set; }
        public string DefaultSelectedTopKey { get; set; }
        public string BackendMainUrl { get; set; }
        public string Avatar { get; set; }
    }
}