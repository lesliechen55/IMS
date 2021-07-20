using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class PurchaseOrderPresentRequestValidator : AbstractValidator<PurchaseOrderPresentRequest>
    {
        public PurchaseOrderPresentRequestValidator()
        {
            RuleFor(x => x.OrderId).Must(x => x > 0);
            RuleFor(x => x.SkuId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty().Must(x => x > 0 && x <= 100);
        }
    }
}