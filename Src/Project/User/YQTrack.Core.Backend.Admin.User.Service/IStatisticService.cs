using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.DTO.Output;

namespace YQTrack.Core.Backend.Admin.User.Service
{
    public interface IStatisticService : IScopeService
    {
        Task<IEnumerable<OrderByDayOutput>> GetOrderByDayDataAsync(int stateType, int dayType);
    }
}