using FluentValidation;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request.Validator
{
    public class ManagerEditRequestValidator : AbstractValidator<ManagerEditRequest>
    {
        public ManagerEditRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.NickName).NotNull().NotEmpty().Length(1, 16);
            RuleFor(x => x.Password).Must(x => string.IsNullOrEmpty(x) || AppConfig.PasswordRegex.IsMatch(x)).WithMessage("最少8个字符和最多16个字符至少1个大写字母，1个小写字母，1个数字和1个特殊字符");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password);
            RuleFor(x => x.Remark).NotEmpty().Length(1, 32);
        }
    }
}