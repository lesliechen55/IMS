using FluentValidation;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Models.Request.Validators
{
    public class ChangePwdRequestValidator : AbstractValidator<ChangePwdRequest>
    {
        public ChangePwdRequestValidator()
        {
            RuleFor(x => x.OldPassword).NotEmpty().Length(8, 16).Must(x => AppConfig.PasswordRegex.IsMatch(x)).WithMessage("最少8个字符和最多16个字符至少1个大写字母，1个小写字母，1个数字和1个特殊字符").NotEqual(x => x.NewPassword).WithMessage("新旧密码不能一致");
            RuleFor(x => x.NewPassword).NotEmpty().Length(8, 16).Must(x => AppConfig.PasswordRegex.IsMatch(x)).WithMessage("最少8个字符和最多16个字符至少1个大写字母，1个小写字母，1个数字和1个特殊字符");
            RuleFor(x => x.ConfirmPassword).NotEmpty().Length(8, 16).Equal(x => x.NewPassword).WithMessage("确认密码必须与新密码保持一致");
        }
    }
}