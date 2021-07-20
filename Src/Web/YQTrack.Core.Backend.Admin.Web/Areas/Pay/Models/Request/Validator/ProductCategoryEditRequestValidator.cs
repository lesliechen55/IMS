using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ProductCategoryEditRequestValidator : AbstractValidator<ProductCategoryEditRequest>
    {
        public ProductCategoryEditRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().Must(x => x > 0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Desc).NotEmpty().MaximumLength(255);
        }
    }
}