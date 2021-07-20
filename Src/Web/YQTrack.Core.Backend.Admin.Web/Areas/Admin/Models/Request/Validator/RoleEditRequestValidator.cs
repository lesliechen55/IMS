using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request.Validator
{
    public class RoleEditRequestValidator : AbstractValidator<RoleEditRequest>
    {
        public RoleEditRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().Length(1, 16);
            RuleFor(x => x.Remark).NotEmpty().Length(1, 32);
        }
    }
}