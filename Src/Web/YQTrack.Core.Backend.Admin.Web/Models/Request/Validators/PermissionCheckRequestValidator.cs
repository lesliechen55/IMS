using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Models.Request.Validators
{
    public class PermissionCheckRequestValidator : AbstractValidator<PermissionCheckRequest>
    {
        public PermissionCheckRequestValidator()
        {
            RuleFor(x => x.Account).NotNull().NotEmpty().Length(1, 16);
            RuleFor(x => x.Password).NotNull().NotEmpty().Length(8, 16).Must(x => AppConfig.PasswordRegex.IsMatch(x)).WithMessage("最少8个字符和最多16个字符至少1个大写字母，1个小写字母，1个数字和1个特殊字符");
            RuleFor(x => x.Ip).NotEmpty();
            RuleFor(x => x.PlatForm).NotEmpty();
            RuleFor(x => x.UserAgent).NotEmpty();
            RuleFor(x => x.PermissionCode).NotEmpty().Must(x =>
            {
                var list = JsonConvert.DeserializeObject<List<string>>(x);
                return list != null && list.Any();
            }).WithMessage("必须包含至少一个权限代码");
        }
    }
}