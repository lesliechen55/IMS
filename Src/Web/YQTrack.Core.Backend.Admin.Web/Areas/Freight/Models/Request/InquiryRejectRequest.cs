using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request
{
    public class InquiryRejectRequest
    {
        [DisplayName("询价单ID")]
        public long Id { get; set; }

        [DisplayName("驳回原因")]
        public string Reason { get; set; }
    }
}