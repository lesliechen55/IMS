using AutoMapper;
using AutoMapper.QueryableExtensions;
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
    public class ReconcileService : IReconcileService
    {
        private readonly PayDbContext _dbContext;
        private readonly IMapper _mapper;

        public ReconcileService(PayDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取对账分页列表
        /// </summary>
        /// <param name="input">商品列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<ReconcilePageDataOutput> outputs, int total)> GetPageDataAsync(ReconcilePageDataInput input)
        {
            var where = PredicateBuilder.True<TReconcile>();
            
            if (input.PaymentProvider.Length > 0)
            {
                var subWhere = PredicateBuilder.False<TReconcile>();
                foreach (var item in input.PaymentProvider)
                {
                    subWhere = subWhere.Or(o => o.FProviderId == item);
                }
                where = where.And(subWhere);
            }
            var queryable = _dbContext.TReconcile
                .Where(where)
                .WhereIf(() => input.StartTime.HasValue, w => w.FBeginTime >= input.StartTime.Value.ToUniversalTime())
                .WhereIf(() => input.EndTime.HasValue, w => w.FBeginTime <= input.EndTime.Value.AddDays(1).ToUniversalTime())
                .ProjectTo<ReconcilePageDataOutput>();
            var count = await queryable.CountAsync();
            var outputs = await queryable
                .OrderByDescending(x => x.FBeginTime)
                .ToPage(input.Page, input.Limit)
                .ToListAsync();
            return (outputs, count);
        }

        /// <summary>
        /// 根据ID获取对账条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ReconcileItemShowOutput>> GetItemByIdAsync(long id)
        {
            List<ReconcileItemShowOutput> output = await _dbContext.TReconcileItem
                .Where(w => w.FReconcileId == id)
                .ProjectTo<ReconcileItemShowOutput>()
                .OrderByDescending(x => x.FUpdateAt)
                .ToListAsync();
            return output;
        }
    }
}
