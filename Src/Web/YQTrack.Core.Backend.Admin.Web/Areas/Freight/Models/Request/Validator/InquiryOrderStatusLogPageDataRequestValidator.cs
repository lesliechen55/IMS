using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request.Validator
{
    public class InquiryOrderStatusLogPageDataRequestValidator : AbstractValidator<InquiryOrderStatusLogPageDataRequest>
    {
        public InquiryOrderStatusLogPageDataRequestValidator()
        {
            RuleFor(x => x.OrderId).Must(x => !x.HasValue || x.Value > 0).WithMessage("询价单ID不能小于等于0");

            RuleFor(x => x.StartTime).Custom((x, y) =>
            {
                if (y.InstanceToValidate is InquiryOrderStatusLogPageDataRequest request)
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
                if (y.InstanceToValidate is InquiryOrderStatusLogPageDataRequest request)
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