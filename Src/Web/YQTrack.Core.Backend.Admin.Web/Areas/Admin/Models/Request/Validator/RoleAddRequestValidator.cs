using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request.Validator
{
    public class RoleAddRequestValidator : AbstractValidator<RoleAddRequest>
    {
        public RoleAddRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 16);
            RuleFor(x => x.Remark).NotEmpty().Length(1, 32);
        }
    }
}