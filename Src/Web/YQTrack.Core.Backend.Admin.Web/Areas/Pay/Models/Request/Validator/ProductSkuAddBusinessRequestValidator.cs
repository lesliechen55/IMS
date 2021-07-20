using FluentValidation;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ProductSkuAddBusinessRequestValidator : AbstractValidator<ProductSkuAddBusinessRequest>
    {
        public ProductSkuAddBusinessRequestValidator()
        {
            RuleFor(x => x.ProductSkuId).NotEmpty().Must(x => x > 0);
            RuleFor(x => x.BusinessCtrlType).NotNull().NotEqual(x => BusinessCtrlType.None);
            RuleFor(x => x.ConsumeType).NotNull().NotEqual(x => ConsumeType.None);
            RuleFor(x => x.Renew).NotNull();
            RuleFor(x => x.Validity).NotEmpty().Must(x => x > 0);
            RuleFor(x => x.Quantity).NotEmpty().Must(x => x > 0);
        }
    }
}