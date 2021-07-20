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
    public class ActivityService : IActivityService
    {
        private readonly PayDbContext _dbContext;
        private readonly IMapper _mapper;

        public ActivityService(PayDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取商品分页列表
        /// </summary>
        /// <param name="input">商品列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<ActivityPageDataOutput> outputs, int total)> GetPageDataAsync(ActivityPageDataInput input)
        {
            var where = PredicateBuilder.True<TActivity>();
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                where = where.And(a => a.FCnName.Contains(input.Keyword));
            }

            if (input.ActivityType.HasValue && input.ActivityType!=-1)
            {
                where = where.And(a => System.Convert.ToInt32(a.FActivityType)==input.ActivityType);
            }
            if (input.ProductId.HasValue)
            {
                where = where.And(a => System.Convert.ToInt32(a.FProductId) == input.ProductId);
            }
            if (input.ActivityStatus.HasValue && input.ActivityStatus!=-1)
            {
                where = where.And(a => System.Convert.ToInt32(a.FStatus) == input.ActivityStatus);
            }

            if (input.StartTime.HasValue)
            {
                where = where.And(a => a.FStartTime >= input.StartTime.Value);
            }
            if (input.EndTime.HasValue)
            {
                where = where.And(a => a.FEndTime >= input.EndTime.Value);
            }

            var queryable = _dbContext.Activities.AsNoTracking().Where(where);
            var count = await queryable.CountAsync();
            var outputs = await queryable
                .OrderByDescending(x => x.FCreateAt)
                .ToPage(input.Page, input.Limit).ToListAsync();
            
            var results = _mapper.Map<IEnumerable<ActivityPageDataOutput>>(outputs);
            return (results, count);
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActivityPageDataOutput> GetByIdAsync(int id)
        {
            var model = await _dbContext.Activities.SingleOrDefaultAsync(x => x.FActivityId == id);
            if (null == model)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(model)}数据不存在");
            }
            var output = _mapper.Map<ActivityPageDataOutput>(model);
            return output;
        }

        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<bool> AddActivityAsync(ActivityEditInput input, int operatorId)
        {
            TActivity model = _mapper.Map<TActivity>(input);

            if (await _dbContext.Activities.AnyAsync(a => a.FCnName == input.CnName))
            {
                throw new BusinessException($"{nameof(input.CnName)}参数错误,该活动名称已存在");
            }

            await _dbContext.Activities.AddAsync(model);
            return await _dbContext.SaveChangesAsync(operatorId) > 0;
        }

        /// <summary>
        /// 修改优惠活动
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<bool> EditAsync(ActivityEditInput input, int operatorId)
        {
            TActivity model = await _dbContext.Activities.SingleOrDefaultAsync(x => x.FActivityId == input.ActivityId);
            if (model == null)
            {
                throw new BusinessException($"{nameof(input.ProductId)}参数错误,{nameof(model)}数据不存在");
            }
            if (await _dbContext.Activities.AnyAsync(a => a.FCnName == input.CnName && a.FActivityId != input.ActivityId))
            {
                throw new BusinessException($"{nameof(input.CnName)}参数错误,该活动名称已存在");
            }

            if (model.FStatus != YQTrack.Backend.Payment.Model.Enums.ActivityStatus.None && model.FStatus != YQTrack.Backend.Payment.Model.Enums.ActivityStatus.Awaiting)
            {
                throw new BusinessException("该状态不支持编辑!");
            }

            model.FCnName = input.CnName;
            model.FEnName = input.EnName;
            model.FDescription = input.Description;
            model.FActivityType = input.ActivityType;//活动类型
            model.FCouponMode = input.CouponMode;//优惠模式
            model.FBusinessType = input.BusinessType;//业务类型
            model.FProductId = input.ProductId;//产品ID
            model.FSkuCodes = input.SkuCodes;
            model.FStartTime = input.FStartTime;
            model.FEndTime = input.FEndTime;
            model.FStatus = input.Status;//活动状态
            model.FDiscountType = input.DiscountType;//优惠类型
            model.FRules = input.Rules;
            model.FTerm = input.Term;
            model.FInternalUse = input.FInternalUse;

            model.FUpdateAt = System.DateTime.UtcNow;
            model.FUpdateBy = operatorId;

            _dbContext.Activities.Update(model);
            return await _dbContext.SaveChangesAsync(operatorId) > 0;
        }

        /// <summary>
        /// 修改优惠活动状态
        /// </summary>
        /// <param name="productId">优惠活动ID</param>
        /// <param name="active">状态</param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<bool> ChangeStatusAsync(ActivityEditStatusInput input, int operatorId)
        {
            TActivity model = await _dbContext.Activities.SingleOrDefaultAsync(x => x.FActivityId == input.ActivityId);
            if (model == null)
            {
                throw new BusinessException($"{nameof(input.ActivityId)}参数错误,{nameof(model)}数据不存在");
            }

            model.FUpdateAt = System.DateTime.Now;
            model.FUpdateBy = operatorId;
            model.FStatus = input.Status;

            _dbContext.Activities.Update(model);
            return await _dbContext.SaveChangesAsync(operatorId) > 0;
        }
    }
}
