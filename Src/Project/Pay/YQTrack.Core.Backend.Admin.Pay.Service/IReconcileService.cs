using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IReconcileService : IScopeService
    {
        /// <summary>
        /// 获取订单分页列表
        /// </summary>
        /// <param name="input">订单列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<ReconcilePageDataOutput> outputs, int total)> GetPageDataAsync(ReconcilePageDataInput input);
        /// <summary>
        /// 根据ID获取对账条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ReconcileItemShowOutput>> GetItemByIdAsync(long id);
    }
}
