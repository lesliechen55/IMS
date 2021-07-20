using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Deals.DTO.Input;
using YQTrack.Core.Backend.Admin.Deals.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Deals.Service
{
    public interface IStatisticsService : IScopeService
    {
        Task<StatisticsAllListOutput> GetStatisticsListDataAsync(StatisticsServiceInput input);

        Task<IEnumerable<StatisticsAllOutput>> GetPageDataContrastDataAsync(StatisticsServiceInput input);

        Task<IEnumerable<StatisticsMerOutput>> GetStatisticsMerStartDataAsync(StatisticsServiceInput input);
        Task<IEnumerable<StatisticsMerOutput>> GetStatisticsMerEndDataAsync(StatisticsServiceInput input);

    }
}
