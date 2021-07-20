using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class InvoiceApplyRejectRequestValidator : AbstractValidator<InvoiceApplyRejectRequest>
    {
        public InvoiceApplyRejectRequestValidator()
        {
            RuleFor(x => x.InvoiceApplyId).Must(x => x > 0);
            RuleFor(x => x.RejectReason).NotEmpty().MaximumLength(64);
            RuleFor(x => x.Remark).MaximumLength(50);
        }
    }
}