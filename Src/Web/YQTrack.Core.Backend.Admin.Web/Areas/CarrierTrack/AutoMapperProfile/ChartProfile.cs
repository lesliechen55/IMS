using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.AutoMapperProfile
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