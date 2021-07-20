using FluentValidation;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request.Validator
{
    public class ImageRequestValidator : AbstractValidator<ImageRequest>
    {
        public ImageRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull().Must(x => x > 0);
            RuleFor(x => x.FormFile).Custom((x, y) =>
            {
                var (success, msg) = FileHelper.CheckFile(x, 2, true, FileExtension.jpg, FileExtension.jpeg, FileExtension.png);
                if (!success)
                {
                    y.AddFailure(msg);
                }
            });
        }
    }
}