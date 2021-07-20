using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using YQTrack.Backend.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class ProductSkuService : IProductSkuService
    {
        private readonly PayDbContext _payDbContext;
        private readonly IMapper _mapper;

        public ProductSkuService(PayDbContext payDbContext, IMapper mapper)
        {
            _payDbContext = payDbContext;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<ProductSkuPageDataOutput> outputs, int total)> GetPageDataAsync(ProductSkuPageDataInput input)
        {
            if (input.ProductType.HasValue)
            {
                if (!await _payDbContext.TProduct.AnyAsync(x => x.FProductId == input.ProductType.Value))
                {
                    throw new BusinessException($"参数{nameof(input.ProductType)}值错误,找不到数据");
                }
            }

            var queryable = _payDbContext.TProductSku
                .WhereIf(() => !input.Name.IsNullOrWhiteSpace(), x => x.FName.Contains(input.Name.Trim()))
                .WhereIf(() => input.Code.IsNotNullOrWhiteSpace(), x => x.FCode == input.Code.Trim())
                .WhereIf(() => input.Desc.IsNotNullOrWhiteSpace(), x => x.FDescription.Contains(input.Desc.Trim()))
                .WhereIf(() => input.ProductType.HasValue && input.ProductType.Value > 0, x => x.FProductId == input.ProductType.Value);

            var count = await queryable.CountAsync();

            var outputs = await queryable
                .OrderByDescending(x => x.FUpdateAt)
                .ToPage(input.Page, input.Limit)
                .Select(x => new ProductSkuPageDataOutput
                {
                    FActive = x.FActive,
                    FCode = x.FCode,
                    FCreateAt = x.FCreateAt,
                    FDescription = x.FDescription,
                    FName = x.FName,
                    FProductSkuId = x.FProductSkuId,
                    FUpdateAt = x.FUpdateAt,
                    ProductName = x.FProduct.FName,
                    ProductCategoryName = x.FProduct.FProductCategory.FName,
                    FMemberLevel = x.FMemberLevel,
                    PriceCount = x.TProductSkuPrice.Count,
                    SkuType = x.FType,
                    IsInternalUse = x.FIsInternalUse
                }).ToListAsync();

            return (outputs, count);
        }

        public async Task<Dictionary<long, string>> GetAllProductAsync()
        {
            var dictionary = await _payDbContext.TProduct.OrderBy(x => x.FName).ToDictionaryAsync(x => x.FProductId, x => x.FName);
            return dictionary;
        }

        public async Task<Dictionary<string, string>> GetSkuListByProductIdAsync(long productId)
        {
            var dictionary = await _payDbContext.TProductSku.Where(p=>
                    p.FProductId == productId && 
                    p.FIsInternalUse == false && 
                    p.FActive)
                .OrderBy(x => x.FName)
                .ToDictionaryAsync(x => x.FCode, x => x.FName);
            return dictionary;
        }

        public async Task AddAsync(ProductSkuAddInput input, int id)
        {
            if (await _payDbContext.TProductSku.AnyAsync(x => x.FCode == input.Code))
            {
                throw new BusinessException($"参数{nameof(input.Code)}错误:已经被占用了");
            }
            var sku = _mapper.Map<TProductSku>(input);
            await _payDbContext.TProductSku.AddAsync(sku);
            await _payDbContext.SaveChangesAsync(id);
        }

        public async Task<ProductSkuEditOutput> GetEditOutputAsync(long skuId)
        {
            var sku = await GetRequiredProductSkuAsync(skuId);
            var output = _mapper.Map<ProductSkuEditOutput>(sku);
            return output;
        }

        private async Task<TProductSku> GetRequiredProductSkuAsync(long skuId)
        {
            var sku = await _payDbContext.TProductSku.SingleOrDefaultAsync(x => x.FProductSkuId == skuId);
            if (sku == null)
            {
                throw new BusinessException($"参数{nameof(skuId)}错误,找不到数据");
            }
            return sku;
        }

        public async Task<IEnumerable<ProductSkuPriceOutput>> GetPriceListAsync(long skuId)
        {
            var sku = await GetRequiredProductSkuAsync(skuId);
            var skuPrices = await _payDbContext.TProductSkuPrice
                .Where(x => x.FProductSkuId == sku.FProductSkuId)
                .OrderBy(x => x.FCreateAt)
                .ProjectTo<ProductSkuPriceOutput>()
                .ToListAsync();
            return skuPrices;
        }

        public async Task<IEnumerable<ProductSkuBusinessOutput>> GetBusinessListAsync(long skuId)
        {
            var sku = await GetRequiredProductSkuAsync(skuId);
            var businesses = JsonConvert.DeserializeObject<IEnumerable<ProductSkuBusinessOutput>>(sku.FBusiness ?? string.Empty);
            return businesses ?? new List<ProductSkuBusinessOutput>();
        }

        public async Task EditAsync(ProductSkuEditInput input, int operatorId)
        {
            var sku = await GetRequiredProductSkuAsync(input.ProductSkuId);
            if (await _payDbContext.TProductSku.AnyAsync(x => x.FCode == input.Code && x.FProductSkuId != sku.FProductSkuId))
            {
                throw new BusinessException($"参数{nameof(input.Code)}已经被占用,请重新编辑提交");
            }
            _mapper.Map(input, sku);
            await _payDbContext.SaveChangesAsync(operatorId);
        }

        public async Task AddPriceAsync(ProductSkuAddPriceInput input, int operatorId)
        {
            if (await _payDbContext.TProductSkuPrice.AnyAsync(x => x.FProductSkuId == input.FProductSkuId && x.FCurrencyType == input.FCurrencyType && x.FPlatformType == input.FPlatformType))
            {
                throw new BusinessException($"当前SKU已经存在相同货币类型和平台类型的价格信息,请核对之后重新提交");
            }
            var skuPrice = _mapper.Map<TProductSkuPrice>(input);
            await _payDbContext.TProductSkuPrice.AddAsync(skuPrice);
            await _payDbContext.SaveChangesAsync(operatorId);
        }

        public async Task ChangeStatusAsync(long skuId, bool enable, int operatorId)
        {
            var sku = await GetRequiredProductSkuAsync(skuId);
            if (enable)
            {
                if (!await _payDbContext.TProductSkuPrice.AnyAsync(x => x.FProductSkuId == sku.FProductSkuId))
                    throw new BusinessException($"{sku.FName}找不到任何价格信息不能启用");
            }
            sku.FActive = enable;
            await _payDbContext.SaveChangesAsync(operatorId);
        }

        public async Task DeletePriceAsync(long skuId, long priceId, int operatorId)
        {
            var price = await _payDbContext.TProductSkuPrice.SingleOrDefaultAsync(x => x.FProductSkuPriceId == priceId && x.FProductSkuId == skuId);
            if (price == null) throw new BusinessException($"参数{nameof(skuId)}或者{nameof(priceId)}错误,找不到数据库数据");
            _payDbContext.TProductSkuPrice.Remove(price);
            await _payDbContext.SaveChangesAsync(operatorId);
        }

        public async Task AddBusinessAsync(ProductSkuAddBusinessInput input, int operatorId)
        {
            var sku = await GetRequiredProductSkuAsync(input.ProductSkuId);
            if (sku.FActive)
            {
                throw new BusinessException($"SKU启用之后不能增删业务信息");
            }

            var businesses = JsonConvert.DeserializeObject<List<ProductSkuBusinessOutput>>(sku.FBusiness ?? string.Empty) ?? new List<ProductSkuBusinessOutput>();

            if (businesses.Any(x => x.BusinessCtrlType == input.BusinessCtrlType))
            {
                throw new BusinessException($"错误:当前SKU已经存在相同的业务类型");
            }

            if (businesses.Any(x => x.ConsumeType != input.ConsumeType || x.Renew != input.Renew || x.Validity != input.Validity))
            {
                throw new BusinessException($"错误:多条业务需要约束消费类型/是否续费/有效期保持完全一致");
            }

            var map = _mapper.Map<ProductSkuBusinessOutput>(input);

            businesses.Add(map);

            sku.FBusiness = JsonConvert.SerializeObject(businesses);

            await _payDbContext.SaveChangesAsync(operatorId);
        }

        public async Task DeleteBusinessAsync(long skuId, BusinessCtrlType businessCtrlType, int loginManagerId)
        {
            var sku = await GetRequiredProductSkuAsync(skuId);
            if (sku.FActive)
            {
                throw new BusinessException($"SKU启用之后不能增删业务信息");
            }
            var businesses = JsonConvert.DeserializeObject<List<ProductSkuBusinessOutput>>(sku.FBusiness ?? string.Empty) ?? new List<ProductSkuBusinessOutput>();

            if (businesses.All(x => x.BusinessCtrlType != businessCtrlType))
            {
                throw new BusinessException($"参数{nameof(businessCtrlType)}错误,没找到对应数据");
            }

            businesses.RemoveAll(x => x.BusinessCtrlType == businessCtrlType);

            sku.FBusiness = JsonConvert.SerializeObject(businesses);

            await _payDbContext.SaveChangesAsync(loginManagerId);
        }
    }
}