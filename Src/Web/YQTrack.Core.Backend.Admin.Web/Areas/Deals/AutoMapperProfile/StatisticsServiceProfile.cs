using YQTrack.Core.Backend.Admin.Deals.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Deals.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Deals.AutoMapperProfile
{
    public class StatisticsServiceProfile : BaseProfile
    {
        public StatisticsServiceProfile()
        {
            CreateMap<SerieItemOutput, SerieItemResponses>();
            CreateMap<StatisticsAllListOutput, StatisticsAllListResponses>();

            CreateMap<StatisticsAllOutput, StatisticsAllResponse>();
            

            CreateMap<StatisticsMerOutput, StatisticsMerResponses>();
        }
    }
}
