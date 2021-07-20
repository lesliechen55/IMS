using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ProductEditRequestValidator : AbstractValidator<ProductEditRequest>
    {
        public ProductEditRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).MaximumLength(255);
            RuleFor(x => x.Role).NotEmpty();
            RuleFor(x => x.ServiceType).NotEmpty();

            RuleFor(x => x.IsSubscription).NotNull();
        }
    }
}