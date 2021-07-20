using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request.Validator
{
    public class ExportUserMarkLogRequestValidator : AbstractValidator<ExportUserMarkLogRequest>
    {
        public ExportUserMarkLogRequestValidator()
        {
            RuleFor(x => x.StartTime).Custom((startTime, y) =>
            {
                if (y.InstanceToValidate is ExportUserMarkLogRequest request)
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

            RuleFor(x => x.EndTime).Custom((endTime, y) =>
            {
                if (y.InstanceToValidate is ExportUserMarkLogRequest request)
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