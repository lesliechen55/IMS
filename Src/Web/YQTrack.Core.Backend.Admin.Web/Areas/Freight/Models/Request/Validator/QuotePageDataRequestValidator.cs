using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request.Validator
{
    public class QuotePageDataRequestValidator : AbstractValidator<QuotePageDataRequest>
    {
        public QuotePageDataRequestValidator()
        {
            RuleFor(x => x.QuoteId).Must(x => !x.HasValue || x.Value > 0).WithMessage("询价单ID不能小于等于0");

            RuleFor(x => x.StartTime).Custom((x, y) =>
            {
                if (y.InstanceToValidate is QuotePageDataRequest request)
                {
                    if (x.HasValue && request.EndTime.HasValue)
                    {
                        if (x.Value.Date > request.EndTime.Value.Date)
                        {
                            y.AddFailure($"发布开始日期不能大于结束日期");
                        }
                    }
                }
            });

            RuleFor(x => x.EndTime).Custom((x, y) =>
            {
                if (y.InstanceToValidate is QuotePageDataRequest request)
                {
                    if (x.HasValue && request.StartTime.HasValue)
                    {
                        if (request.StartTime.Value.Date > x.Value.Date)
                        {
                            y.AddFailure($"发布开始日期不能大于结束日期");
                        }
                    }
                }
            });
        }
    }
}