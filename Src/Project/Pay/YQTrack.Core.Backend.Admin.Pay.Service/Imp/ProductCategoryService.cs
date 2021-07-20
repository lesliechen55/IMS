using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly PayDbContext _payDbContext;
        private readonly IMapper _mapper;

        public ProductCategoryService(PayDbContext payDbContext,
            IMapper mapper)
        {
            _payDbContext = payDbContext;
            _mapper = mapper;
        }
        public async Task<List<ProductCategoryOutput>> GetAllDataAsync()
        {
            return await _payDbContext.TProductCategory.ProjectTo<ProductCategoryOutput>().ToListAsync();
        }
        public async Task<(IEnumerable<ProductCategoryPageDataOutput> Outputs, int Total)> GetPageDataAsync(ProductCategoryPageDataInput input)
        {
            var queryable = _payDbContext.TProductCategory
                .WhereIf(() => !input.Name.IsNullOrWhiteSpace(), x => x.FName.Contains(input.Name.Trim()));

            var count = await queryable.CountAsync();

            var list = await queryable.OrderByDescending(x => x.FUpdateAt).ToPage(input.Page, input.Limit).Select(x => new ProductCategoryPageDataOutput
            {
                FProductCategoryId = x.FProductCategoryId,
                FCode = x.FCode,
                FCreateAt = x.FCreateAt,
                FCreateBy = x.FCreateBy,
                FDescription = x.FDescription,
                FName = x.FName,
                FUpdateAt = x.FUpdateAt,
                FUpdateBy = x.FUpdateBy,
                ProductCount = x.TProduct.Count
            }).ToListAsync();

            return (list, count);
        }

        public async Task AddAsync(ProductCategoryAddInput input, int createBy)
        {
            if (await _payDbContext.TProductCategory.AnyAsync(x => x.FCode == input.Code.Trim() || x.FName == input.Name.Trim()))
            {
                throw new BusinessException($"参数{nameof(input.Code)}或者{nameof(input.Name)}不允许重复");
            }
            var productCategory = _mapper.Map<TProductCategory>(input);
            await _payDbContext.TProductCategory.AddAsync(productCategory);
            await _payDbContext.SaveChangesAsync(createBy);
        }

        public async Task<ProductCategoryOutput> GetByIdAsync(long id)
        {
            var category = await GetRequiredAsync(id);
            var output = _mapper.Map<ProductCategoryOutput>(category);
            return output;
        }

        private async Task<TProductCategory> GetRequiredAsync(long id)
        {
            var category = await _payDbContext.TProductCategory.SingleOrDefaultAsync(x => x.FProductCategoryId == id);
            if (category == null)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(category)}数据库不存在");
            }
            return category;
        }

        public async Task EditAsync(long id, string name, string code, string desc, int operatorId)
        {
            var category = await GetRequiredAsync(id);

            if (await _payDbContext.TProductCategory.AnyAsync(x => x.FProductCategoryId != id && (x.FCode == code || x.FName == name)))
            {
                throw new BusinessException($"参数{nameof(code)}或者{nameof(name)}已经被其他分类所使用,请重新修改后重试");
            }

            category.FName = name;
            category.FCode = code;
            category.FDescription = desc;
            await _payDbContext.SaveChangesAsync(operatorId);
        }
    }
}