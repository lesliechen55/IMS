using System.Linq;
using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request.Validator
{
    public class ManagerSetRoleRequestValidator : AbstractValidator<ManagerSetRoleRequest>
    {
        public ManagerSetRoleRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.RoleIdList).NotNull().Must(x => x.Any()).WithMessage(x => $"{nameof(x.RoleIdList)}至少包含一元素,角色不能为空");
        }
    }
}