using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class PaymentSelectResponse
    {
        public static Dictionary<int, string> PlatformType => EnumHelper.GetSelectItem<UserPlatformType>();
        public static Dictionary<int, string> CurrencyType => EnumHelper.GetSelectItem<CurrencyType>();
        public static Dictionary<int, string> ServiceType => EnumHelper.GetSelectItem<ServiceType>();
        public static Dictionary<int, string> PaymentProvider => EnumHelper.GetSelectItem<PaymentProvider>();
        public static Dictionary<int, string> PaymentStatus => EnumHelper.GetSelectItem<PaymentStatus>(true);

        public PaymentPageDataRequest Request { get; set; }
    }
}
