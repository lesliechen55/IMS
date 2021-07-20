using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request.Validator
{
    public class CarrierTrackUserEditRequestValidator : AbstractValidator<CarrierTrackUserEditRequest>
    {
        public CarrierTrackUserEditRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.ExportTimeLimit).GreaterThanOrEqualTo(5000);
            RuleFor(x => x.ImportTodayLimit).GreaterThanOrEqualTo(5000);
        }
    }
}