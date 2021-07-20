using System.Collections.Generic;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ReconcileSelectResponse
    {
        public static Dictionary<int, string> PaymentProvider => EnumHelper.GetSelectItem<PaymentProvider>();
    }
}
