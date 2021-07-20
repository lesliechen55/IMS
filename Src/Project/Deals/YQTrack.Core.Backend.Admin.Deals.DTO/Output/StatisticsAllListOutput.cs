namespace YQTrack.Core.Backend.Admin.Deals.DTO.Output
{
    public class StatisticsAllListOutput
    {
        public ChartOutput ClickRate { get; set; }
        public ChartOutput TransactionRate { get; set; }
        public ChartOutput PaymentRate { get; set; }
        public ChartOutput ConversionRate { get; set; }
        public ChartOutput ECPCRate { get; set; }
        public ChartOutput AllRate { get; set; }
    }
}
