using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ChangeStatusRequest
    {
        [DisplayName("SKU-ID")]
        public long SkuId { get; set; }

        [DisplayName("是否启用")]
        public bool? Enable { get; set; }
    }
}