using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request.Validator
{
    public class ExportRequestValidator : AbstractValidator<ExportRequest>
    {
        public ExportRequestValidator()
        {
            RuleFor(x => x.StartTime).NotEmpty().Custom((startTime, y) =>
            {
                if (y.InstanceToValidate is ExportRequest request)
                {
                    if (startTime.HasValue && request.EndTime.HasValue)
                    {
                        if (startTime.Value.Date > request.EndTime.Value.Date)
                        {
                            y.AddFailure($"开始日期不能大于结束日期");
                        }
                    }
                }
            });

            RuleFor(x => x.EndTime).NotEmpty().Custom((endTime, y) =>
            {
                if (y.InstanceToValidate is ExportRequest request)
                {
                    if (endTime.HasValue && request.StartTime.HasValue)
                    {
                        if (request.StartTime.Value.Date > endTime.Value.Date)
                        {
                            y.AddFailure($"开始日期不能大于结束日期");
                        }
                    }
                }
            });
        }
    }
}