using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ProductSkuPageDataRequestValidator : AbstractValidator<ProductSkuPageDataRequest>
    {
        public ProductSkuPageDataRequestValidator()
        {
            RuleFor(x => x.ProductType).Must(x => !x.HasValue || x.Value > 0);
        }
    }
}