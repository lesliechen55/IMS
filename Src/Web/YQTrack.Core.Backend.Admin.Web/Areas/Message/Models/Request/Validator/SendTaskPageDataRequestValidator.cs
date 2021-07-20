using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request.Validator
{
    public class SendTaskPageDataRequestValidator : AbstractValidator<SendTaskPageDataRequest>
    {
        public SendTaskPageDataRequestValidator()
        {
            RuleFor(x => x.StartTime).Custom((x, y) =>
            {
                if (y.InstanceToValidate is SendTaskPageDataRequest request)
                {
                    if (x.HasValue && request.EndTime.HasValue)
                    {
                        if (x.Value.Date > request.EndTime.Value.Date)
                        {
                            y.AddFailure($"开始日期不能大于结束日期");
                        }
                    }
                }
            });

            RuleFor(x => x.EndTime).Custom((x, y) =>
            {
                if (y.InstanceToValidate is SendTaskPageDataRequest request)
                {
                    if (x.HasValue && request.StartTime.HasValue)
                    {
                        if (request.StartTime.Value.Date > x.Value.Date)
                        {
                            y.AddFailure($"开始日期不能大于结束日期");
                        }
                    }
                }
            });
        }
    }
}