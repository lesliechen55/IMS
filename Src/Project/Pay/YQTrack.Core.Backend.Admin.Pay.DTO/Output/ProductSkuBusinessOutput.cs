using Newtonsoft.Json;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class ProductSkuBusinessOutput
    {
        [JsonProperty(PropertyName = "t")]
        public BusinessCtrlType BusinessCtrlType { get; set; }

        [JsonProperty("c")]
        public ConsumeType ConsumeType { get; set; }

        [JsonProperty("r")]
        public bool Renew { get; set; }

        [JsonProperty("v")]
        public byte Validity { get; set; }

        [JsonProperty("q")]
        public int Quantity { get; set; }
    }
}