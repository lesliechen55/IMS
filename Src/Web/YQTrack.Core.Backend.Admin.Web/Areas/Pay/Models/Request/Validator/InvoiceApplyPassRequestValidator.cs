using FluentValidation;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class InvoiceApplyPassRequestValidator : AbstractValidator<InvoiceApplyPassRequest>
    {
        public InvoiceApplyPassRequestValidator()
        {
            RuleFor(x => x.InvoiceApplyId).Must(x => x > 0);
            RuleFor(x => x.SendType).IsInEnum().NotEqual(SendType.None);
            RuleFor(x => x.SendInfo).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Remark).MaximumLength(50);
        }
    }
}