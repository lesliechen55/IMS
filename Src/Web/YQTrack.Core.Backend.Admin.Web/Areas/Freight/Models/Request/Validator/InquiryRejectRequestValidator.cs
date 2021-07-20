using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request.Validator
{
    public class InquiryRejectRequestValidator : AbstractValidator<InquiryRejectRequest>
    {
        public InquiryRejectRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Reason).NotEmpty().MaximumLength(300);
        }
    }
}