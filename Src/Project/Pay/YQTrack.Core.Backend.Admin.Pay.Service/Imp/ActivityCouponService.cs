using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.CommonService;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class ActivityCouponService : IActivityCouponService
    {
        private readonly PayDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserInfoService _userInfoService;

        public ActivityCouponService(PayDbContext dbContext, IMapper mapper, IUserInfoService userInfoService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// 获取优惠券分页列表
        /// </summary>
        /// <param name="input">商品列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<ActivityCouponPageDataOutput> outputs, int total)> GetPageDataAsync(ActivityCouponPageDataInput input)
        {
            var where = PredicateBuilder.True<TActivityCoupon>();
            //if (input.ActivityId.HasValue)
            //{
            //    where = where.And(a => a.FActivityId == input.ActivityId);
            //}

            if (!string.IsNullOrWhiteSpace(input.ActivityName))
            {
                where = where.And(a => a.FActivity.FCnName.Contains(input.ActivityName));
            }

            if (input.PurchaseOrderId.HasValue)
            {
                where = where.And(a => a.FPurchaseOrderId == input.PurchaseOrderId);
            }

            if (input.UserId.HasValue)
            {
                where = where.And(a => a.FUserId == input.UserId);
            }
            if (!string.IsNullOrWhiteSpace(input.UserEmail))
            {
                where = where.And(a => a.FEmail == input.UserEmail);
            }

            if (input.Status.HasValue && input.Status != -1)
            {
                where = where.And(a => Convert.ToInt32(a.FStatus) == input.Status);
            }

            var queryable = _dbContext.ActivityCoupons
                .AsNoTracking().Where(where);
            var count = await queryable.CountAsync();

            //var outputs = await queryable
            //    .OrderByDescending(x => x.FCreateAt)
            //    .ToPage(input.Page, input.Limit).ToListAsync();

            var outputs = await queryable
                .OrderByDescending(x => x.FCreateAt)
                .ToPage(input.Page, input.Limit)
                .Select(s => new ActivityCouponPageDataOutput
                {
                    FActivityCouponId = s.FActivityCouponId,
                    FActivityId = s.FActivityId,
                    FActivityName = s.FActivity.FCnName,
                    FPurchaseOrderId = s.FPurchaseOrderId,
                    FUserId = s.FUserId,
                    FEmail = s.FEmail,
                    FStartTime = s.FStartTime,
                    FEndTime = s.FEndTime,
                    FStatus = s.FStatus,
                    FRule = s.FRule,
                    FSource = s.FSource,
                    FActualDiscount = s.FActualDiscount.HasValue ? (Convert.ToDecimal(s.FActualDiscount)) : 0
                }).ToListAsync();

            var results = _mapper.Map<IEnumerable<ActivityCouponPageDataOutput>>(outputs);
            return (results, count);
        }

        /// <summary>
        /// 发放优惠券
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<int> AddActivityCouponAsync(ActivityCouponEditInput input, int operatorId)
        {
            var activitie = await _dbContext.Activities.SingleOrDefaultAsync(x => x.FActivityId == input.ActivityId);
            if (activitie == null)
            {
                throw new BusinessException("该活动已不存在");
            }
            if (activitie.FStatus != YQTrack.Backend.Payment.Model.Enums.ActivityStatus.Active)
            {
                throw new BusinessException("只有已上架的活动才可以发放优惠券");
            }

            #region 日期逻辑
            DateTime sTime = DateTime.UtcNow;
            DateTime eTime = activitie.FEndTime.ToUniversalTime();
            if (activitie.FTerm > 0)
            {
                //领取时间加上这里固定输入的天数
                eTime = sTime.AddDays(activitie.FTerm);
            }
            else if (activitie.FTerm == -1) //长期有效
            {
                eTime = Convert.ToDateTime("9999-01-01");
            }
            else
            {
                sTime = activitie.FStartTime;//跟随活动
                sTime = sTime.ToUniversalTime();
            }
            #endregion

            //检查用户Email是否正确
            long? userId = await _userInfoService.GetUserIdByEmailAsync(input.Email);
            if (userId == null)
            {
                throw new BusinessException("用户Email输入不正确,请检查!");
            }

            JArray jo = (JArray)JsonConvert.DeserializeObject(input.Rule);

            foreach (var item in jo)
            {
                TActivityCoupon model = _mapper.Map<TActivityCoupon>(input);

                var str = item.ToString();
                model.FRule = Regex.Replace(str, @"\s", "");
                model.FActivityCouponId = IdHelper.GetGenerateId();
                model.FUserId = userId.Value;
                model.FCreateBy = operatorId;

                model.FStatus = YQTrack.Backend.Payment.Model.Enums.CouponStatus.Unused;//优惠券状态为1未使用
                model.FSource = "17Track";//暂时没有这个类型哦

                model.FStartTime = sTime;
                model.FEndTime = eTime;

                await _dbContext.ActivityCoupons.AddAsync(model);
            }

            return await _dbContext.SaveChangesAsync(operatorId);
        }

    }
}
