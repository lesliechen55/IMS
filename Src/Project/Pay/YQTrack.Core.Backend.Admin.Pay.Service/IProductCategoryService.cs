using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IProductCategoryService : IScopeService
    {
        Task<List<ProductCategoryOutput>> GetAllDataAsync();
        Task<(IEnumerable<ProductCategoryPageDataOutput> Outputs, int Total)> GetPageDataAsync(ProductCategoryPageDataInput input);

        [OperationTracePlus(desc: "添加商品分类", type: OperationType.Add)]
        Task AddAsync(ProductCategoryAddInput input, int createBy);

        Task<ProductCategoryOutput> GetByIdAsync(long id);

        [OperationTracePlus(desc: "修改商品分类", type: OperationType.Edit)]
        Task EditAsync(long id, string name, string code, string desc, int operatorId);
    }
}