using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Freight.Service.RemoteApi
{
    public class RejectInquiryRequest
    {
        public string Version { get; set; }
        public string Method { get; set; }
        public Dictionary<string, string> Param { get; set; }
    }
}


