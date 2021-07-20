using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class InvoicePageDataRequest : PageRequest
    {
        /// <summary>
        /// 注册邮箱
        /// </summary>
        public string UserEmail { get; set; }
    }
}