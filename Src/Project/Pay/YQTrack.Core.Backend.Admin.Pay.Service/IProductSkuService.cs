using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Backend.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IProductSkuService : IScopeService
    {
        Task<(IEnumerable<ProductSkuPageDataOutput> outputs, int total)> GetPageDataAsync(ProductSkuPageDataInput input);

        Task<Dictionary<long, string>> GetAllProductAsync();

        Task<Dictionary<string, string>> GetSkuListByProductIdAsync(long productId);

        [OperationTracePlus(desc: "添加SKU", type: OperationType.Add)]
        Task AddAsync(ProductSkuAddInput input, int id);

        Task<ProductSkuEditOutput> GetEditOutputAsync(long skuId);

        Task<IEnumerable<ProductSkuPriceOutput>> GetPriceListAsync(long skuId);

        Task<IEnumerable<ProductSkuBusinessOutput>> GetBusinessListAsync(long skuId);

        [OperationTracePlus(desc: "编辑SKU", type: OperationType.Edit)]
        Task EditAsync(ProductSkuEditInput input, int operatorId);

        [OperationTracePlus(desc: "添加SKU价格信息", type: OperationType.Add)]
        Task AddPriceAsync(ProductSkuAddPriceInput input, int operatorId);

        [OperationTracePlus(desc: "修改SKU启用禁用状态", type: OperationType.Edit)]
        Task ChangeStatusAsync(long skuId, bool enable, int operatorId);

        [OperationTracePlus(desc: "删除SKU价格信息", type: OperationType.Delete)]
        Task DeletePriceAsync(long skuId, long priceId, int operatorId);

        [OperationTracePlus(desc: "添加SKU业务信息", type: OperationType.Add)]
        Task AddBusinessAsync(ProductSkuAddBusinessInput input, int operatorId);

        [OperationTracePlus(desc: "删除SKU业务信息", type: OperationType.Delete)]
        Task DeleteBusinessAsync(long skuId, BusinessCtrlType businessCtrlType, int loginManagerId);
    }
}