using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request.Validator
{
    public class SendTemplateTestRequestValidator : AbstractValidator<SendTemplateTestRequest>
    {
        public SendTemplateTestRequestValidator()
        {
            RuleFor(x => x.Remarks).NotEmpty().MaximumLength(200);
            RuleFor(x => x.TemplateId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.ObjDetails).NotEmpty().MaximumLength(4000);
        }
    }
}