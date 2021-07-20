using System.Collections.Generic;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class InvoiceApplyPassResponse
    {
        public long InvoiceApplyId { get; set; }
        public string InvoiceEmail { get; set; }
        public Dictionary<int, string> SendType => EnumHelper.GetSelectItem<SendType>();
    }
}
