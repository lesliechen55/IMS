using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IManualReconcileService : IScopeService
    {
        Task<bool> ExistAsync(string md5);

        Task ImportGlocashAsync(string json, string fileName, string md5, string storagePath, int year, int month, string remark, int operatorId);

        Task<(IEnumerable<ManualReconcilePageDataOutput> outputs, int total)> GetPageDataAsync(ManualReconcilePageDataInput input);
    }
}