using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class OrderChartRequestValidator : AbstractValidator<OrderChartRequest>
    {
        public OrderChartRequestValidator()
        {
            RuleFor(x => x.OrderAnalysisType).IsInEnum();
            RuleFor(x => x.ChartDateType).NotNull().IsInEnum();
            RuleFor(x => x.CurrencyType).IsInEnum();
            RuleFor(x => x.ExchangeRate).Matches(@"^(?!(0\d*$))\d+\.?\d{0,4}$");
        }
    }
}