using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.DevOps.DTO;

namespace YQTrack.Core.Backend.Admin.DevOps.Service
{
    public interface ITrackInfoKeyFilterDeleteService : IScopeService
    {
        /// <summary>
        /// 批量删除跟踪单号缓存Key
        /// </summary>
        /// <param name="filter"></param>
        [OperationTracePlus(desc: "根据模糊搜索批量删除跟踪单号缓存", type: OperationType.Delete)]
        void BatchDelete(string filter);

        /// <summary>
        /// 获取批量删除是否已完成
        /// </summary>
        /// <returns></returns>
        (BatchDeleteState batchDeleteState, string filter) GetBatchDeleteState();

        /// <summary>
        /// 获取已批量删除的缓存键
        /// </summary>
        /// <returns></returns>
        (BatchDeleteState batchDeleteState, DeleteData deleteData) GetBatchDeleteKeys(string key);

        /// <summary>
        /// 取消批量删除跟踪单号缓存
        /// </summary>
        /// <returns></returns>
        [OperationTracePlus(desc: "取消批量删除跟踪单号缓存", type: OperationType.Edit)]
        void CancelBatchDelete();
    }
}
