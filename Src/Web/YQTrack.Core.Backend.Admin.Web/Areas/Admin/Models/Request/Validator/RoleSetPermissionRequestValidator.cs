using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request.Validator
{
    public class RoleSetPermissionRequestValidator : AbstractValidator<RoleSetPermissionRequest>
    {
        public RoleSetPermissionRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.PermissionIdList).NotEmpty().WithMessage(x => $"{nameof(x.PermissionIdList)}至少包含一元素,权限不能为空");
        }
    }
}