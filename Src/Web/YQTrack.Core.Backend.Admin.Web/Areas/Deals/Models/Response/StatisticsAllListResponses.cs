namespace YQTrack.Core.Backend.Admin.Web.Areas.Deals.Models.Response
{
    public class StatisticsAllListResponses
    {
        public ChartResponses ClickRate { get; set; }
        public ChartResponses TransactionRate { get; set; }
        public ChartResponses PaymentRate { get; set; }
        public ChartResponses ConversionRate { get; set; }
        public ChartResponses ECPCRate { get; set; }
        public ChartResponses AllRate { get; set; }
    }
}
