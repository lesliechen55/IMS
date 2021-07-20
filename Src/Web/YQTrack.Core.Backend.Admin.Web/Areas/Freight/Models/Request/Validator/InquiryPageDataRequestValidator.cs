using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request.Validator
{
    public class InquiryPageDataRequestValidator : AbstractValidator<InquiryPageDataRequest>
    {
        public InquiryPageDataRequestValidator()
        {
            RuleFor(x => x.Id).Must(x => !x.HasValue || x.Value > 0).WithMessage("询价单ID不能小于等于0");

            RuleFor(x => x.PublishStartTime).Custom((x, y) =>
            {
                if (y.InstanceToValidate is InquiryPageDataRequest request)
                {
                    if (x.HasValue && request.PublishEndTime.HasValue)
                    {
                        if (x.Value.Date > request.PublishEndTime.Value.Date)
                        {
                            y.AddFailure($"发布开始日期不能大于结束日期");
                        }
                    }
                }
            });

            RuleFor(x => x.PublishEndTime).Custom((x, y) =>
            {
                if (y.InstanceToValidate is InquiryPageDataRequest request)
                {
                    if (x.HasValue && request.PublishStartTime.HasValue)
                    {
                        if (request.PublishStartTime.Value.Date > x.Value.Date)
                        {
                            y.AddFailure($"发布开始日期不能大于结束日期");
                        }
                    }
                }
            });

            RuleFor(x => x.ExpireStartTime).Custom((x, y) =>
            {
                if (y.InstanceToValidate is InquiryPageDataRequest request)
                {
                    if (x.HasValue && request.ExpireEndTime.HasValue)
                    {
                        if (x.Value.Date > request.ExpireEndTime.Value.Date)
                        {
                            y.AddFailure($"过期开始日期不能大于结束日期");
                        }
                    }
                }
            });

            RuleFor(x => x.ExpireEndTime).Custom((x, y) =>
            {
                if (y.InstanceToValidate is InquiryPageDataRequest request)
                {
                    if (x.HasValue && request.ExpireStartTime.HasValue)
                    {
                        if (request.ExpireStartTime.Value.Date > x.Value.Date)
                        {
                            y.AddFailure($"过期开始日期不能大于结束日期");
                        }
                    }
                }
            });
        }
    }
}