using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class ProductService : IProductService
    {
        private readonly PayDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductService(PayDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取商品分页列表
        /// </summary>
        /// <param name="input">商品列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<ProductPageDataOutput> outputs, int total)> GetPageDataAsync(ProductPageDataInput input)
        {
            var where = PredicateBuilder.True<TProduct>();
            if (input.ProductCategory.HasValue)
            {
                where = where.And(a => a.FProductCategoryId == input.ProductCategory.Value);
            }
            if (input.ServiceType.Length > 0)
            {
                var subWhere = PredicateBuilder.False<TProduct>();
                foreach (var item in input.ServiceType)
                {
                    subWhere = subWhere.Or(o => o.FServiceType == item);
                }
                where = where.And(subWhere);
            }
            if (input.Role.Length > 0)
            {
                var subWhere = PredicateBuilder.False<TProduct>();
                foreach (var item in input.Role)
                {
                    subWhere = subWhere.Or(o => o.FRole == item);
                }
                where = where.And(subWhere);
            }
            if (input.Keyword.IsNotNullOrWhiteSpace())
            {
                where = where.And(a => a.FName.Contains(input.Keyword) || a.FCode.Contains(input.Keyword) || a.FDescription.Contains(input.Keyword));
            }
            var queryable = _dbContext.TProduct.AsNoTracking().Where(where);
            var count = await queryable.CountAsync();
            var outputs = await queryable
                .OrderByDescending(x => x.FUpdateAt)
                .ToPage(input.Page, input.Limit)
                .Select(s => new ProductPageDataOutput
                {
                    FProductId = s.FProductId,
                    CategoryName = s.FProductCategory.FName,
                    FActive = s.FActive,
                    FCode = s.FCode,
                    FName = s.FName,
                    FDescription = s.FDescription,
                    FRole = s.FRole,
                    FServiceType = s.FServiceType,
                    FCreateAt = s.FCreateAt,
                    FCreateBy = s.FCreateBy,
                    FUpdateAt = s.FUpdateAt,
                    FUpdateBy = s.FUpdateBy,
                    SkuCount = s.TProductSku.Count,
                    FIsSubscription = s.FIsSubscription
                }).ToListAsync();
            return (outputs, count);
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ProductShowOutput> GetByIdAsync(int id)
        {
            var model = await _dbContext.TProduct.SingleOrDefaultAsync(x => x.FProductId == id);
            if (null == model)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(model)}数据不存在");
            }
            var output = _mapper.Map<ProductShowOutput>(model);
            return output;
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ProductEditInput input, int operatorId)
        {
            if (await _dbContext.TProduct.AnyAsync(a => a.FCode == input.Code))
            {
                throw new BusinessException($"{nameof(input.Code)}参数错误,该代号已存在");
            }
            if (await _dbContext.TProduct.AnyAsync(a => a.FName == input.Name))
            {
                throw new BusinessException($"{nameof(input.Name)}参数错误,该商品名称已存在");
            }
            TProduct model = _mapper.Map<TProduct>(input);
            model.FActive = false;
            //model.FProductId = 0;
            await _dbContext.TProduct.AddAsync(model);
            return await _dbContext.SaveChangesAsync(operatorId) > 0;
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<bool> EditAsync(ProductEditInput input, int operatorId)
        {
            TProduct model = await _dbContext.TProduct.SingleOrDefaultAsync(x => x.FProductId == input.ProductId);
            if (model == null)
            {
                throw new BusinessException($"{nameof(input.ProductId)}参数错误,{nameof(model)}数据不存在");
            }
            if (await _dbContext.TProduct.AnyAsync(a => a.FCode == input.Code && a.FProductId != input.ProductId))
            {
                throw new BusinessException($"{nameof(input.Code)}参数错误,该代号已存在");
            }
            if (await _dbContext.TProduct.AnyAsync(a => a.FName == input.Name && a.FProductId != input.ProductId))
            {
                throw new BusinessException($"{nameof(input.Name)}参数错误,该商品名称已存在");
            }
            //model.FActive = input.Active;
            model.FDescription = input.Description;
            model.FCode = input.Code;
            model.FName = input.Name;
            model.FRole = input.Role;
            model.FProductCategoryId = input.ProductCategoryId;
            model.FServiceType = input.ServiceType;
            model.FIsSubscription = input.IsSubscription;
            _dbContext.TProduct.Update(model);
            return await _dbContext.SaveChangesAsync(operatorId) > 0;
        }

        /// <summary>
        /// 修改商品状态
        /// </summary>
        /// <param name="productId">商品ID</param>
        /// <param name="active">是否启动</param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task ChangeStatusAsync(long productId, bool active, int operatorId)
        {
            TProduct model = await _dbContext.TProduct.Include(i => i.TProductSku).SingleOrDefaultAsync(x => x.FProductId == productId);
            if (model == null)
            {
                throw new BusinessException($"{nameof(productId)}参数错误,{nameof(model)}数据不存在");
            }
            if (active && !model.TProductSku.Any())
            {
                throw new BusinessException($"{nameof(active)}参数错误,{nameof(model)}SKU数据不存在，不能启用");
            }
            model.FActive = active;
            await _dbContext.SaveChangesAsync(operatorId);
        }
    }
}
