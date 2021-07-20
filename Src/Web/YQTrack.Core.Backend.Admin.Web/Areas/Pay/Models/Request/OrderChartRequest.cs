using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class OrderChartRequest
    {
        public OrderAnalysisType? OrderAnalysisType { get; set; }
        public ChartDateType? ChartDateType { get; set; }
        public string Email { get; set; }
        public ServiceType[] ServiceType { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public string ExchangeRate { get; set; }
        public UserPlatformType[] PlatformType { get; set; }
        public PurchaseOrderStatus[] OrderStatus { get; set; }
    }
}