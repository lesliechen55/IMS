using System.ComponentModel;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ProductSkuAddBusinessRequest
    {
        [DisplayName("SKU-ID")]
        public long ProductSkuId { get; set; }

        [DisplayName("业务控制类型")]
        public BusinessCtrlType? BusinessCtrlType { get; set; }

        [DisplayName("消费类型")]
        public ConsumeType? ConsumeType { get; set; }

        [DisplayName("是否续费")]
        public bool? Renew { get; set; } = false;

        [DisplayName("有效期")]
        public byte Validity { get; set; }

        [DisplayName("业务数量")]
        public int Quantity { get; set; }
    }
}