using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request.Validator
{
    public class CarrierTrackUserAddRequestValidator : AbstractValidator<CarrierTrackUserAddRequest>
    {
        public CarrierTrackUserAddRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().MaximumLength(256);
            RuleFor(x => x.ExportTimeLimit).GreaterThanOrEqualTo(5000);
            RuleFor(x => x.ImportTodayLimit).GreaterThanOrEqualTo(5000);
        }
    }
}