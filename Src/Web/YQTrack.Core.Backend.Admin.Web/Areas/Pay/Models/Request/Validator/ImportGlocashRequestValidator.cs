using FluentValidation;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ImportGlocashRequestValidator : AbstractValidator<ImportGlocashRequest>
    {
        public ImportGlocashRequestValidator()
        {
            RuleFor(x => x.FormFile).NotNull().Must(x =>
            {
                var (success, msg) = FileHelper.CheckFile(x, 20, false, FileExtension.json);
                return success;
            }).WithMessage("文件大小或者格式验证不通过");

            RuleFor(x => x.Year).NotEmpty().Must(x => x.HasValue && x.Value >= 2019 && x.Value <= 2050).WithMessage("最小年份必须大于等于2019年");

            RuleFor(x => x.Month).NotEmpty().Must(x => x.HasValue && x.Value >= 1 && x.Value <= 12).WithMessage("必须是有效月份");

            RuleFor(x => x.Remark).NotEmpty().MaximumLength(50);
        }
    }
}