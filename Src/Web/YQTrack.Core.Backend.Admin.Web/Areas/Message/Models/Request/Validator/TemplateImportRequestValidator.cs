using FluentValidation;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request.Validator
{
    public class TemplateImportRequestValidator : AbstractValidator<TemplateImportRequest>
    {
        public TemplateImportRequestValidator()
        {
            RuleFor(x => x.FormFile).Must(x =>
            {
                if (1 > 0)
                {
                    var (success, msg) = FileHelper.CheckFile(x, 2, true, FileExtension.js, FileExtension.json);
                    return success;
                }
            }).WithMessage("文件格式验证不通过");
        }
    }
}
