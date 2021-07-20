using Newtonsoft.Json;

namespace YQTrack.Core.Backend.Admin.DevOps.DTO
{
    public class TrackSummary
    {
        [JsonProperty(PropertyName = "s")]
        public int Source { get; set; }

        [JsonProperty(PropertyName = "c")]
        public int CarrierType { get; set; }
    }
}
