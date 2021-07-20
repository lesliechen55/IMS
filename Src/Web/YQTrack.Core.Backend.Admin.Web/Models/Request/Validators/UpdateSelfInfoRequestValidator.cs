using System;
using FluentValidation;

namespace YQTrack.Core.Backend.Admin.Web.Models.Request.Validators
{
    public class UpdateSelfInfoRequestValidator : AbstractValidator<UpdateAvatarRequest>
    {
        public UpdateSelfInfoRequestValidator()
        {
            RuleFor(x => x.FormFile).NotNull().Must(x => (
                x.FileName.EndsWith("jpg", StringComparison.InvariantCultureIgnoreCase) ||
                x.FileName.EndsWith("png", StringComparison.InvariantCultureIgnoreCase) ||
                x.FileName.EndsWith("jpeg", StringComparison.InvariantCultureIgnoreCase))
                && x.Length < 2 * 1024 * 1024).WithMessage("您必须上传图片且大小不能超过2M");
        }
    }
}