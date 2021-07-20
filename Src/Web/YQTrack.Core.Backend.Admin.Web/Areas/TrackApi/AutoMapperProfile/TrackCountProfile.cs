using YQTrack.Core.Backend.Admin.TrackApi.DTO.Input;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.AutoMapperProfile
{
    public class TrackCountProfile : BaseProfile
    {
        public TrackCountProfile()
        {
            CreateMap<TrackCountRequest, TrackCountInput>();
            CreateMap<TrackCountOutput, TrackCountResponse>().ForMember(
                    dest => dest.Date,
                    opt => opt.MapFrom(src => src.FDate.ToString("yyyy-MM-dd"))
                );
        }
    }
}
