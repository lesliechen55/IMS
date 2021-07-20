using FluentValidation;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ProductSkuAddRequestValidator : AbstractValidator<ProductSkuAddRequest>
    {
        public ProductSkuAddRequestValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().Must(x => x > 0);
            RuleFor(x => x.MemberLevel).NotNull().IsInEnum();
            RuleFor(x => x.SkuType).NotNull().IsInEnum();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(32);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Desc).NotEmpty().MaximumLength(255);

            RuleFor(x => x.IsInternalUse).NotNull().Custom((x, y) =>
            {
                if (x.HasValue && x.Value && y.InstanceToValidate is ProductSkuAddRequest request)
                {
                    if (request.SkuType.HasValue && request.SkuType.Value != SkuType.Email &&
                        request.SkuType.Value != SkuType.Track)
                    {
                        y.AddFailure($"只能SKU类型为邮件数或者查询数才能启用内部使用开关!");
                    }
                }
            });
        }
    }
}