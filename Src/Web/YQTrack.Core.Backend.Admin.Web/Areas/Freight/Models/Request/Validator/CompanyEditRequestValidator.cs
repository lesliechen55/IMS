using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request.Validator
{
    public class CompanyEditRequestValidator : AbstractValidator<CompanyEditRequest>
    {
        public CompanyEditRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Limit).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().MaximumLength(32);
            RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Url).NotEmpty().MaximumLength(100);
        }
    }
}