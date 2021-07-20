using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Seller.DTO.Input;
using YQTrack.Core.Backend.Admin.Seller.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Seller.Service
{
    public interface ITrackBatchTaskService : IScopeService
    {
        /// <summary>
        /// 获取大批量任务分页列表
        /// </summary>
        /// <param name="input">大批量任务列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<BatchTaskPageDataOutput> outputs, int total)> GetPageDataAsync(BatchTaskPageDataInput input);
    }
}
