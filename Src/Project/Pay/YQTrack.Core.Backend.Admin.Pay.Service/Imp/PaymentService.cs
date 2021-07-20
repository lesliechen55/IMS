using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.CommonService;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class PaymentService : IPaymentService
    {
        private readonly PayDbContext _dbContext;
        private readonly IUserInfoService _userInfoService;
        private readonly IMapper _mapper;

        public PaymentService(PayDbContext dbContext, IUserInfoService userInfoService, IMapper mapper)
        {
            _dbContext = dbContext;
            _userInfoService = userInfoService;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取支付分页列表
        /// </summary>
        /// <param name="input">商品列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<PaymentPageDataOutput> outputs, int total)> GetPageDataAsync(PaymentPageDataInput input)
        {
            var where = PredicateBuilder.True<TPayment>();
            if (input.PlatformType.Length > 0)
            {
                var subWhere = PredicateBuilder.False<TPayment>();
                foreach (var item in input.PlatformType)
                {
                    subWhere = subWhere.Or(o => o.FPlatformType == item);
                }
                where = where.And(subWhere);
            }
            if (input.ServiceType.Length > 0)
            {
                var subWhere = PredicateBuilder.False<TPayment>();
                foreach (var item in input.ServiceType)
                {
                    subWhere = subWhere.Or(o => o.FServiceType == item);
                }
                where = where.And(subWhere);
            }
            if (input.PaymentProvider.Length > 0)
            {
                var subWhere = PredicateBuilder.False<TPayment>();
                foreach (var item in input.PaymentProvider)
                {
                    subWhere = subWhere.Or(o => o.FProviderId == item);
                }
                where = where.And(subWhere);
            }
            if (input.PaymentStatus.Length > 0)
            {
                var subWhere = PredicateBuilder.False<TPayment>();
                foreach (var item in input.PaymentStatus)
                {
                    subWhere = subWhere.Or(o => o.FPaymentStatus == item);
                }
                where = where.And(subWhere);
            }
            long? userId = await _userInfoService.GetUserIdByEmailAsync(input.Email);
            var queryable = _dbContext.TPayment
                .Where(where)
                .WhereIf(() => input.OrderId.HasValue, w => w.FOrderId == input.OrderId)
                .WhereIf(() => userId.HasValue, w => w.FPayerId == userId)
                .WhereIf(() => input.UserId.HasValue, w => w.FPayerId == input.UserId)
                .WhereIf(() => input.Name.IsNotNullOrWhiteSpace(), w => w.FOrderName.Contains(input.Name))
                .WhereIf(() => input.CurrencyType.HasValue, w => w.FCurrencyType == input.CurrencyType)
                .WhereIf(() => input.StartTime.HasValue, w => w.FUpdateAt >= input.StartTime.Value.ToUniversalTime())
                .WhereIf(() => input.EndTime.HasValue, w => w.FUpdateAt <= input.EndTime.Value.AddDays(1).ToUniversalTime())
                .ProjectTo<PaymentPageDataOutput>();
            var count = await queryable.CountAsync();
            var outputs = await queryable
                .OrderByDescending(x => x.FUpdateAt)
                .ToPage(input.Page, input.Limit)
                .ToListAsync();
            return (outputs, count);
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PaymentShowOutput> GetByIdAsync(long id)
        {
            var model = await _dbContext.TPayment.SingleOrDefaultAsync(x => x.FOrderId == id);
            if (null == model)
            {
                throw new BusinessException($"订单：{id},支付记录不存在");
            }
            var output = _mapper.Map<PaymentShowOutput>(model);
            output.PaymentLog = await _dbContext.TPaymentLog
                .Where(w => w.FOrderId == model.FOrderId.Value.ToString())
                .OrderByDescending(o => o.FUpdateAt)
                .ProjectTo<PaymentLogOutput>()
                .ToListAsync();
            return output;
        }

        /// <summary>
        /// 根据ID获取订单是否可退款
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(PaymentProvider provider, PaymentStatus status)> GetRefundByIdAsync(long id)
        {
            var data = await _dbContext.TPayment.Where(w => w.FOrderId == id).Select(s => new { s.FProviderId, s.FPaymentStatus }).SingleOrDefaultAsync();
            return (data.FProviderId, data.FPaymentStatus);
        }
    }
}
