using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request.Validator
{
    public class OperationLogPageDataRequestValidator : AbstractValidator<OperationLogPageDataRequest>
    {
        public OperationLogPageDataRequestValidator()
        {
            RuleFor(x => x.StartDate).Custom((x, y) =>
            {
                if (y.InstanceToValidate is OperationLogPageDataRequest request)
                {
                    if (x.HasValue && request.EndDate.HasValue)
                    {
                        if (x.Value.Date > request.EndDate.Value.Date)
                        {
                            y.AddFailure($"开始日期不能大于结束日期");
                        }
                    }
                }
            });

            RuleFor(x => x.EndDate).Custom((x, y) =>
            {
                if (y.InstanceToValidate is OperationLogPageDataRequest request)
                {
                    if (x.HasValue && request.StartDate.HasValue)
                    {
                        if (request.StartDate.Value.Date > x.Value.Date)
                        {
                            y.AddFailure($"开始日期不能大于结束日期");
                        }
                    }
                }
            });
        }
    }
}