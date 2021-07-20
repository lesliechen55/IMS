using FluentValidation;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ProductSkuAddPriceRequestValidator : AbstractValidator<ProductSkuAddPriceRequest>
    {
        public ProductSkuAddPriceRequestValidator()
        {
            RuleFor(x => x.ProductSkuId).NotEmpty().Must(x => x > 0);
            RuleFor(x => x.CurrencyType).NotNull().NotEqual(x => CurrencyType.None);
            RuleFor(x => x.Description).MaximumLength(255);
            RuleFor(x => x.PlatformType).NotNull().NotEqual(x => UserPlatformType.UNKNOWN);
            RuleFor(x => x.SaleUnitPrice).Must(x => x > 0);
            RuleFor(x => x.Amount).Must(x => x > 0);
        }
    }
}