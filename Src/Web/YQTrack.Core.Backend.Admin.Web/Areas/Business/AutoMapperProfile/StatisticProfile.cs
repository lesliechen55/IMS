using YQTrack.Core.Backend.Admin.Log.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.AutoMapperProfile
{
    public class StatisticProfile : BaseProfile
    {
        public StatisticProfile()
        {
            CreateMap<SerieItemOutput, SerieItemResponse>();
            CreateMap<ChartOutput, ChartResponse>();
        }
    }
}