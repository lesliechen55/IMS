using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Freight.Service
{
    public interface ICompanyService : IScopeService
    {
        Task<(IEnumerable<CompanyPageDataOutput> outputs, int total)> GetCompanyPageDataAsync(CompanyPageDataInput input);
        Task<CompanyEditOutput> GetCompanyEditInfoAsync(long id);

        [OperationTrace(desc: "修改运输商公司信息", type: OperationType.Edit)]
        Task EditAsync(CompanyEditInput input);

        Task<CompanyViewCheckOutput> GetCompanyViewCheckInfoAsync(long id);

        Task<byte[]> GetViewCheckImageAsync(string imgUrl);

        [OperationTrace(desc: "通过运输商公司注册", type: OperationType.Edit)]
        Task PassAsync(long id);

        [OperationTrace(desc: "驳回运输商公司注册", type: OperationType.Edit)]
        Task RejectAsync(long id, string desc);
    }
}