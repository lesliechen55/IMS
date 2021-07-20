using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ChangeProductStatusRequest
    {
        [DisplayName("商品ID")]
        public long ProductId { get; set; }

        [DisplayName("是否启用")]
        public bool? Active { get; set; }
    }
}