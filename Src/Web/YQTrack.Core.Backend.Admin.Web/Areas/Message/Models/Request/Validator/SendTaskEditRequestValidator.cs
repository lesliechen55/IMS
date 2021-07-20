using FluentValidation;
using YQTrack.Core.Backend.Admin.Message.Core.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request.Validator
{
    public class SendTaskEditRequestValidator : AbstractValidator<SendTaskEditRequest>
    {
        public SendTaskEditRequestValidator()
        {
            RuleFor(x => x.Remarks).NotEmpty().MaximumLength(200);
            RuleFor(x => x.TemplateTypeId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.ObjDetails).Custom((x, y) =>
            {
                if (y.InstanceToValidate is SendTaskEditRequest request)
                {
                    if (request.SendType == SendType.ByUser && string.IsNullOrWhiteSpace(x))
                    {
                        y.AddFailure("必填项不能为空");
                    }
                }
            });
        }
    }
}