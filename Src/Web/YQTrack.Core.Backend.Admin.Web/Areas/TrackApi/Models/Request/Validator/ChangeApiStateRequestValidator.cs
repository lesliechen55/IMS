using FluentValidation;
using System;
using YQTrack.Core.Backend.Enums.TrackApi;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request.Validator
{
    public class ChangeApiStateRequestValidator : AbstractValidator<ChangeApiStateRequest>
    {
        public ChangeApiStateRequestValidator()
        {
            RuleFor(x => x.UserNo).NotEmpty().Must(m => m > 10000);
            RuleFor(x => x.ApiState).NotEmpty().Must(m => Enum.IsDefined(typeof(ApiState), m) && m != ApiState.None);
        }
    }
}
