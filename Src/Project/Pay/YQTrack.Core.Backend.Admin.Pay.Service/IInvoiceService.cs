using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IInvoiceService : IScopeService
    {
        /// <summary>
        /// 获取发票资料分页列表
        /// </summary>
        /// <param name="input">发票资料列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<InvoicePageDataOutput> outputs, int total)> GetPageDataAsync(InvoicePageDataInput input);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InvoiceShowOutput> GetByIdAsync(long id);

        /// <summary>
        /// 添加发票资料
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "添加发票资料信息", type: OperationType.Add)]
        Task<bool> AddAsync(InvoiceAddInput input, int operatorId);

        /// <summary>
        /// 修改发票资料
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "修改发票资料信息", type: OperationType.Edit)]
        Task EditAsync(InvoiceEditInput input, int operatorId);
    }
}
