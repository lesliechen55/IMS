using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ChangeStatusRequestValidator : AbstractValidator<ChangeStatusRequest>
    {
        public ChangeStatusRequestValidator()
        {
            RuleFor(x => x.SkuId).NotEmpty().Must(x => x > 0);
            RuleFor(x => x.Enable).NotNull();
        }
    }
}