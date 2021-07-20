using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.TrackApi.Data;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Input;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.TrackApi.Service.Imp
{
    public class TrackCountService : ITrackCountService
    {
        private readonly ApiUserDbContext _dbApiUserContext;

        public TrackCountService(ApiUserDbContext dbApiUserContext)
        {
            _dbApiUserContext = dbApiUserContext;
        }

        /// <summary>
        /// 获取消耗列表
        /// </summary>
        /// <param name="input">消耗列表搜索条件</param>
        /// <returns></returns>
        public async Task<List<TrackCountOutput>> GetListDataAsync(TrackCountInput input)
        {
            var output = await _dbApiUserContext.TApiTrackCount
                .WhereIf(() => input.UserId.HasValue, x => x.FUserId == input.UserId.Value)
                .WhereIf(() => input.StartTime.HasValue, x => x.FDate >= input.StartTime.Value)
                .WhereIf(() => input.EndTime.HasValue, x => x.FDate < input.EndTime.Value.AddDays(1))
                .GroupBy(g => g.FDate)
                .OrderByDescending(o => o.Key)
                .Select(s => new TrackCountOutput
                {
                    FCount = s.Sum(ss => ss.FCount),
                    FDate = s.Key
                })
                //.ProjectTo<TrackCountOutput>()
                .ToListAsync();

            return output;
        }
    }
}
