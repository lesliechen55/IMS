using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class OfflinePaymentPassRequestValidator : AbstractValidator<OfflinePaymentPassRequest>
    {
        public OfflinePaymentPassRequestValidator()
        {
            RuleFor(x => x.OfflinePaymentId).Must(x => x > 0);
            RuleFor(x => x.Remark).MaximumLength(50);
        }
    }
}