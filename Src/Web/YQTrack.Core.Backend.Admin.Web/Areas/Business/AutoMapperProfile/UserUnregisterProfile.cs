using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.AutoMapperProfile
{
    public class UserUnregisterProfile : BaseProfile
    {
        public UserUnregisterProfile()
        {
            CreateMap<UserUnregisterPageDataOutput, UserUnregisterPageDataResponse>()
                .ForMember(
                    dest => dest.UserRole,
                    opt => opt.MapFrom(src => src.FUserRole.GetDescription())
                    )
                .ForMember(
                    dest => dest.UnRegisterTime,
                    opt => opt.MapFrom(src => src.FUnRegisterTime.ToLocalTime())
                )
                .ForMember(
                    dest => dest.CompletedTime,
                    opt => opt.MapFrom(src => src.FCompletedTime.ToLocalTime())
                );
        }
    }
}