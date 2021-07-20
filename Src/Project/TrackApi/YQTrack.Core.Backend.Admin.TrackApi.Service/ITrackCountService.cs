using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Input;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;

namespace YQTrack.Core.Backend.Admin.TrackApi.Service
{
    public interface ITrackCountService : IScopeService
    {
        /// <summary>
        /// 获取消耗列表
        /// </summary>
        /// <param name="input">消耗列表搜索条件</param>
        /// <returns></returns>
        Task<List<TrackCountOutput>> GetListDataAsync(TrackCountInput input);
    }
}
