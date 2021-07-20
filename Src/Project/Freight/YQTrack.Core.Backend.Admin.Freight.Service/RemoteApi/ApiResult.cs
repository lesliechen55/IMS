using Newtonsoft.Json;

namespace YQTrack.Core.Backend.Admin.Freight.Service.RemoteApi
{
    public class ApiResult
    {
        [JsonProperty(PropertyName = "success")]
        public bool Success { get; set; }

        [JsonProperty(PropertyName = "msg")]
        public string Msg { get; set; }

        [JsonProperty(PropertyName = "data")]
        public object Data { get; set; }
    }
}