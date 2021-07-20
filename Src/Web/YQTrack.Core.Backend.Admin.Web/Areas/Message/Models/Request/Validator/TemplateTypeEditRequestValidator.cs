using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request.Validator
{
    public class TemplateTypeEditRequestValidator : AbstractValidator<TemplateTypeEditRequest>
    {
        public TemplateTypeEditRequestValidator()
        {
            RuleFor(x => x.Channel).Must(x => x.HasValue && x.Value > 0);
            //RuleFor(x => x.DataJson).NotEmpty();
            RuleFor(x => x.ProjectId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.TemplateName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.TemplateTitle).MaximumLength(1000);
        }
    }
}
