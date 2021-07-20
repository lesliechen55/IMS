using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.AutoMapperProfile
{
    public class ChartProfile : BaseProfile
    {
        public ChartProfile()
        {
            CreateMap<SerieItemOutput, SerieItemResponses>();
            CreateMap<ChartOutput, ChartResponses>();
        }
    }
}