using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request.Validator
{
    public class ChartRequestValidator : AbstractValidator<ChartRequest>
    {
        public ChartRequestValidator()
        {
            RuleFor(x => x.ChartDateType).NotNull().IsInEnum();
            RuleFor(x => x.UserNo).Must(x => !x.HasValue || x > 0);
        }
    }
}