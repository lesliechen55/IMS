using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request.Validator
{
    public class EmailPageDataRequestValidator : AbstractValidator<EmailPageDataRequest>
    {
        public EmailPageDataRequestValidator()
        {
            RuleFor(x => x.SubmitDateRange).NotEmpty().WithMessage("请选择时间段");

            RuleFor(x => x.SubmitStartTime).NotEmpty().WithMessage("请选择必填项:提交开始时间");
            RuleFor(x => x.SubmitEndTime).NotEmpty().WithMessage("请选择必填项:提交结束时间");
            RuleFor(x => x.To).NotEmpty().WithMessage("请选择必填项:收件人邮箱");

            RuleFor(x => x.SubmitStartTime).Custom((x, y) =>
            {
                if (y.InstanceToValidate is EmailPageDataRequest request)
                {
                    if (x.HasValue && request.SubmitEndTime.HasValue)
                    {
                        if (x > request.SubmitEndTime)
                        {
                            y.AddFailure($"提交开始时间不能大于提交结束时间");
                        }
                    }
                }
            });

            RuleFor(x => x.SubmitEndTime).Custom((x, y) =>
            {
                if (y.InstanceToValidate is EmailPageDataRequest request)
                {
                    if (request.SubmitStartTime.HasValue && x.HasValue)
                    {
                        if (request.SubmitStartTime > x)
                        {
                            y.AddFailure($"提交开始时间不能大于提交结束时间");
                        }
                    }
                }
            });

        }
    }
}