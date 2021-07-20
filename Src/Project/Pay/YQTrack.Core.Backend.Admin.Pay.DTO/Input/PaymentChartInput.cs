using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class PaymentChartInput
    {
        public PaymentAnalysisType? PaymentAnalysisType { get; set; }
        public ChartDateType ChartDateType { get; set; }
        public string Email { get; set; }
        public PaymentProvider[] PaymentProvider { get; set; }
        public ServiceType[] ServiceType { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public string ExchangeRate { get; set; }
        public UserPlatformType[] PlatformType { get; set; }
        public PaymentStatus[] PaymentStatus { get; set; }
    }
}
