using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ManualReconcilePageDataRequestValidator : AbstractValidator<ManualReconcilePageDataRequest>
    {
        public ManualReconcilePageDataRequestValidator()
        {
            RuleFor(x => x.Year).Must(x => !x.HasValue || (x.Value >= 2019 && x.Value <= 2050)).WithMessage("最小年份必须大于等于2019年");

            RuleFor(x => x.Month).Must(x => !x.HasValue || (x.Value >= 1 && x.Value <= 12)).WithMessage("必须是有效月份");
        }
    }
}