using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request.Validator
{
    public class ChartRequestValidator : AbstractValidator<ChartRequest>
    {
        public ChartRequestValidator()
        {
            RuleFor(x => x.ChartDateType).NotNull().IsInEnum();
        }
    }
}