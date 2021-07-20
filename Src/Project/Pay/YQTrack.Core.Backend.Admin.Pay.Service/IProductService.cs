using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service
{
    public interface IProductService : IScopeService
    {
        /// <summary>
        /// 获取商品分页列表
        /// </summary>
        /// <param name="input">商品列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<ProductPageDataOutput> outputs, int total)> GetPageDataAsync(ProductPageDataInput input);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductShowOutput> GetByIdAsync(int id);

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "添加商品信息", type: OperationType.Add)]
        Task<bool> AddAsync(ProductEditInput input, int operatorId);

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "修改商品信息", type: OperationType.Edit)]
        Task<bool> EditAsync(ProductEditInput input, int operatorId);

        /// <summary>
        /// 修改商品状态
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="active">是否启动</param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        [OperationTracePlus(desc: "修改商品状态", type: OperationType.Edit)]
        Task ChangeStatusAsync(long productId, bool active, int operatorId);
    }
}
