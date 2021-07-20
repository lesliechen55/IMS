using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class RefundRequestValidator : AbstractValidator<RefundRequest>
    {
        public RefundRequestValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty().Must(x => x > 0);
            RuleFor(x => x.Reason).NotNull().MaximumLength(20);
        }
    }
}