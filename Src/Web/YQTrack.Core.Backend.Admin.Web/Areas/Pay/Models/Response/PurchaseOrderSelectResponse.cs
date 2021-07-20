using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class PurchaseOrderSelectResponse
    {
        public Dictionary<int, string> PlatformType => EnumHelper.GetSelectItem<UserPlatformType>();
        public Dictionary<int, string> CurrencyType => EnumHelper.GetSelectItem<CurrencyType>();
        public Dictionary<int, string> ServiceType => EnumHelper.GetSelectItem<ServiceType>();
        public Dictionary<int, string> ProviderId => EnumHelper.GetSelectItem<PaymentProvider>();
        public Dictionary<int, string> PurchaseOrderStatus => EnumHelper.GetSelectItem<PurchaseOrderStatus>(true);
        public PurchaseOrderPageDataRequest Request { get; set; }
    }
}
