using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.DevOps.DTO;

namespace YQTrack.Core.Backend.Admin.DevOps.Service
{
    public interface ITrackInfoNumbersDeleteService : IScopeService
    {
        /// <summary>
        /// 获取跟踪单号的完整缓存列表
        /// </summary>
        /// <param name="trackNos">跟踪单号</param>
        /// <returns></returns>
        List<TrackCache> GetListTrackCache(string[] trackNos);

        /// <summary>
        /// 删除跟踪单号缓存Key
        /// </summary>
        /// <param name="keys"></param>
        [OperationTracePlus(desc: "根据完整单号删除跟踪单号缓存", type: OperationType.Delete)]
        void DeleteKeys(string[] keys);

    }
}
