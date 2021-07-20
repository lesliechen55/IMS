using FluentValidation;
using YQTrack.Core.Backend.Enums.TrackApi;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request.Validator
{
    public class UserInfoEditRequestValidator : AbstractValidator<UserInfoEditRequest>
    {
        public UserInfoEditRequestValidator()
        {
            RuleFor(x => x.UserNo).NotEmpty().Must(m => m >= 10000 && m <= short.MaxValue).WithMessage("用户编号为10000~32767之间的数字");
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(5).MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            //RuleFor(x => x.MaxTrackReq).NotEmpty().GreaterThanOrEqualTo(10000);
            //RuleFor(x => x.TrackFrequency).MaximumLength(100);
            RuleFor(x => x.CompanyName).MaximumLength(128);
            RuleFor(x => x.VATNo).MaximumLength(50);
            RuleFor(x => x.Country).MaximumLength(50);
            RuleFor(x => x.Address).MaximumLength(256);
            RuleFor(x => x.ContactName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.ContactPhone).NotEmpty().MaximumLength(20);
            RuleFor(x => x.ContactEmail).NotEmpty().EmailAddress();

            RuleFor(x => x.ScheduleFrequency).NotNull().IsInEnum().Must(x => x != ScheduleFrequency.None);
            RuleFor(x => x.GiftQuota).NotNull().Must(x => x.HasValue && x.Value <= 100 && x.Value >= 0).WithMessage("赠送配额设置范围必须是:0<=x<=100");

        }
    }
}
