using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class OfflinePaymentRejectRequestValidator : AbstractValidator<OfflinePaymentRejectRequest>
    {
        public OfflinePaymentRejectRequestValidator()
        {
            RuleFor(x => x.OfflinePaymentId).Must(x => x > 0);
            RuleFor(x => x.RejectReason).NotEmpty().MaximumLength(1024);
            RuleFor(x => x.Remark).MaximumLength(50);
        }
    }
}