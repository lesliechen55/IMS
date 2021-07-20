using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ChangeProductStatusRequestValidator : AbstractValidator<ChangeProductStatusRequest>
    {
        public ChangeProductStatusRequestValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().Must(x => x > 0);
            RuleFor(x => x.Active).NotNull();
        }
    }
}