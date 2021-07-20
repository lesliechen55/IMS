using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request.Validator
{
    public class TemplateEditRequestValidator : AbstractValidator<TemplateEditRequest>
    {
        public TemplateEditRequestValidator()
        {
            RuleFor(x => x.Language).NotEmpty().MaximumLength(16);
            RuleFor(x => x.TemplateData).NotEmpty().MaximumLength(4000);
            RuleFor(x => x.TemplateBody).NotEmpty();
            RuleFor(x => x.TemplateTitle).MaximumLength(1000);
        }
    }
}
