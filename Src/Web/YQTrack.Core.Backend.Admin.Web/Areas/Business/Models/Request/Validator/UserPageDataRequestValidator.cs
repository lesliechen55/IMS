using FluentValidation;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request.Validator
{
    public class UserPageDataRequestValidator : AbstractValidator<UserPageDataRequest>
    {
        public UserPageDataRequestValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.UserId).Must(x => !x.HasValue || x.Value > 0).Custom((x, y) =>
            {
                if (y.InstanceToValidate is UserPageDataRequest request)
                {
                    if (!x.HasValue && request.Email.IsNullOrWhiteSpace() && request.Gid.IsNullOrWhiteSpace())
                    {
                        y.AddFailure("搜索参数至少包含UserId(Gid)或者Email其一");
                    }
                }
            });

            RuleFor(x => x.Email).Custom((x, y) =>
            {
                if (y.InstanceToValidate is UserPageDataRequest request)
                {
                    if (x.IsNullOrWhiteSpace() && !request.UserId.HasValue && request.Gid.IsNullOrWhiteSpace())
                    {
                        y.AddFailure("搜索参数至少包含UserId(Gid)或者Email其一");
                    }
                }
            });

            RuleFor(x => x.Gid).Custom((x, y) =>
            {
                if (y.InstanceToValidate is UserPageDataRequest request)
                {
                    if (x.IsNullOrWhiteSpace() && !request.UserId.HasValue && request.Email.IsNullOrWhiteSpace())
                    {
                        y.AddFailure("搜索参数至少包含UserId(Gid)或者Email其一");
                    }
                }
            });
        }
    }
}