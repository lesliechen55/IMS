using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Input;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Output;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Service
{
    public interface IHomeService : IScopeService
    {
        Task<IEnumerable<IndexPageDataOutput>> GetPageDataAsync(IndexPageDataInput input);

        [OperationTracePlus("添加货代用户资料", OperationType.Add)]
        Task AddAsync(CarrierTrackUserAddInput input, int operatorId);

        Task<(IndexPageDataOutput output, int availableTrackNum, int buyTotal)> GetByIdAsync(long controlId, long userId);

        [OperationTracePlus("编辑货代用户资料", OperationType.Add)]
        Task EditAsync(long requestId, long userId, int requestImportTodayLimit, int requestExportTimeLimit, bool requestEnable, int loginManagerId);
    }
}