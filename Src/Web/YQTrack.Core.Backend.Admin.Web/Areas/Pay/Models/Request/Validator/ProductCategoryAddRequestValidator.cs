using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ProductCategoryAddRequestValidator : AbstractValidator<ProductCategoryAddRequest>
    {
        public ProductCategoryAddRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Desc).NotEmpty().MaximumLength(255);
        }
    }
}