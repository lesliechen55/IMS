using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ProductPageDataRequestValidator : AbstractValidator<ProductPageDataRequest>
    {
        public ProductPageDataRequestValidator()
        {
            RuleFor(x => x.ProductCategory).Must(x => !x.HasValue || x.Value > 0);
        }
    }
}