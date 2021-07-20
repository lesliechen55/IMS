using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
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