using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.CommonService;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Pay.Service.Imp.Dto;
using YQTrack.Core.Backend.Admin.User.Data.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly PayDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserInfoService _userInfoService;

        public PurchaseOrderService(PayDbContext dbContext, IMapper mapper,
            IUserInfoService userInfoService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userInfoService = userInfoService;
        }

        /// <summary>
        /// 获取订单分页列表
        /// </summary>
        /// <param name="input">商品列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<PurchaseOrderPageDataOutput> outputs, int total)> GetPageDataAsync(
            PurchaseOrderPageDataInput input)
        {
            var where = PredicateBuilder.True<TPurchaseOrder>();
            if (input.PlatformType.Length > 0)
            {
                var subWhere = PredicateBuilder.False<TPurchaseOrder>();
                foreach (var item in input.PlatformType)
                {
                    subWhere = subWhere.Or(o => o.FUserPlatformType == item);
                }
                where = where.And(subWhere);
            }
            if (input.ServiceType.Length > 0)
            {
                var subWhere = PredicateBuilder.False<TPurchaseOrder>();
                foreach (var item in input.ServiceType)
                {
                    subWhere = subWhere.Or(o => o.FServiceType == item);
                }
                where = where.And(subWhere);
            }
            if (input.ProviderId.Length > 0)
            {
                var subWhere = PredicateBuilder.False<TPurchaseOrder>();
                foreach (var item in input.ProviderId)
                {
                    subWhere = subWhere.Or(o => o.FProviderId == item);
                }
                where = where.And(subWhere);
            }
            if (input.PurchaseOrderStatus.Length > 0)
            {
                var subWhere = PredicateBuilder.False<TPurchaseOrder>();
                foreach (var item in input.PurchaseOrderStatus)
                {
                    subWhere = subWhere.Or(o => o.FStatus == item);
                }
                where = where.And(subWhere);
            }
            long? userId = await _userInfoService.GetUserIdByEmailAsync(input.Email);
            var queryable = _dbContext.TPurchaseOrder
                .Where(where)
                .WhereIf(() => input.OrderId.HasValue, w => w.FPurchaseOrderId == input.OrderId)
                .WhereIf(() => userId.HasValue, w => w.FUserId == userId.Value)
                .WhereIf(() => input.UserId.HasValue, w => w.FUserId == input.UserId.Value)
                .WhereIf(() => input.Name.IsNotNullOrWhiteSpace(), w => w.FName.Contains(input.Name))
                .WhereIf(() => input.CurrencyType.HasValue, w => w.FCurrencyType == input.CurrencyType)
                .WhereIf(() => input.StartTime.HasValue, w => w.FUpdateAt >= input.StartTime.Value.ToUniversalTime())
                .WhereIf(() => input.EndTime.HasValue,
                    w => w.FUpdateAt <= input.EndTime.Value.AddDays(1).ToUniversalTime())
                .WhereIf(() => input.ConflictOrder.HasValue, x => x.FIsSubscriptionConflict == input.ConflictOrder.Value)
                .ProjectTo<PurchaseOrderPageDataOutput>();
            var count = await queryable.CountAsync();
            var outputs = await queryable
                .OrderByDescending(x => x.FUpdateAt)
                .ToPage(input.Page, input.Limit)
                .ToListAsync();

            var userNameList = await _userInfoService.GetUserNickName(outputs.Select(x => x.FUserId).ToArray());
            if (userNameList.Count > 0)
            {
                outputs.ForEach(x =>
                {
                    x.FnickName = GetNickName(userNameList,x.FUserId);
                });
            }

            return (outputs, count);
        }

        private string GetNickName(Dictionary<long, string> dict, long userId)
        {
            string nickName = string.Empty;
            if (userId == 0) return nickName;
            try
            {
                nickName = dict[userId];
            }
            catch (Exception){ }
            return nickName;
        }

        private static bool GetShowFlag(TPurchaseOrder model)
        {
            return model.FProviderId != PaymentProvider.Present &&
                   model.FServiceType != ServiceType.Donate && model.FServiceType != ServiceType.Unknown &&
                   model.FServiceType != ServiceType.Buyer &&
                   model.FIsSubscriptionConflict == false &&
                   (model.FStatus == PurchaseOrderStatus.Success ||
                    model.FStatus == PurchaseOrderStatus.Delivering ||
                    model.FStatus == PurchaseOrderStatus.Delivered);
        }

        private static void PresentCheckUserRole(TuserInfo tuserInfo)
        {
            if (!tuserInfo.FuserRole.HasValue ||
                tuserInfo.FuserRole.Value == UserRoleType.None ||
                tuserInfo.FuserRole.Value == UserRoleType.Buyer)
            {
                throw new BusinessException($"{tuserInfo.FuserId}找不到有效符合赠送的角色信息");
            }
        }

        private async Task<TPurchaseOrder> GetRequiredAsync(long id)
        {
            var model = await _dbContext.TPurchaseOrder.SingleOrDefaultAsync(x => x.FPurchaseOrderId == id);
            if (null == model)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(model)}数据不存在");
            }
            return model;
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PurchaseOrderShowOutput> GetByIdAsync(long id)
        {
            var model = await GetRequiredAsync(id);
            await _dbContext.Entry(model).Collection(c => c.TPurchaseOrderItem).Query()
                .OrderByDescending(o => o.FUpdateAt).LoadAsync();
            var output = _mapper.Map<PurchaseOrderShowOutput>(model);
            output.IsShowPresentPage = GetShowFlag(model);
            return output;
        }

        public async Task<(Dictionary<int, string> output, string email)> GetSkuAsync(long userId,
            CurrencyType currencyType)
        {
            var tuserInfo = await _userInfoService.GetRequiredByIdAsync(userId);
            PresentCheckUserRole(tuserInfo);
            var skuList = await (from product in _dbContext.TProduct
                                 join productSku in _dbContext.TProductSku on product.FProductId equals productSku.FProductId
                                 join productSkuPrice in _dbContext.TProductSkuPrice on productSku.FProductSkuId equals productSkuPrice.FProductSkuId
                                 where product.FRole == tuserInfo.FuserRole.Value &&
                                       product.FActive &&
                                       productSku.FActive &&
                                       productSku.FMemberLevel == UserMemberLevel.FreeMember &&
                                       productSkuPrice.FCurrencyType == currencyType
                                 orderby productSku.FCode
                                 select new
                                 {
                                     productSku.FProductSkuId,
                                     productSku.FName,
                                     productSku.FType,
                                     productSku.FBusiness
                                 }).Distinct().ToListAsync();

            var dictionary = skuList.Select(x => new { x.FProductSkuId, FName = GetShowSkuInfo(x.FName, x.FType, x.FBusiness) }).OrderBy(x => x.FName).ToDictionary(x => (int)x.FProductSkuId, x => x.FName);
            return (dictionary, tuserInfo.Femail);
        }

        private static string GetShowSkuInfo(string skuName, SkuType skuType, string business, bool isDescription = true)
        {
            var descriptionOrDisplayName = isDescription ? skuType.GetDescription() : skuType.GetDisplayName();
            var jArray = JsonConvert.DeserializeObject<JArray>(business);
            if (!jArray.Any())
            {
                throw new BusinessException($"{skuName}的业务信息异常,业务信息={business},请检查数据库");
            }
            var jArrayFirst = jArray.First;
            if (jArrayFirst["q"] == null)
            {
                throw new BusinessException($"{skuName}的业务信息异常,业务信息={business},请检查数据库");
            }
            var value = jArrayFirst.Value<int>("q");
            return $"{value} {descriptionOrDisplayName}";
        }

        public async Task<long> PresentAsync(long orderId, int skuId, int quantity, int operatorId)
        {
            var model = await GetRequiredAsync(orderId);

            var isShowPresentPage = GetShowFlag(model);
            if (!isShowPresentPage)
            {
                throw new BusinessException($"当前订单:{orderId}不符合赠送条件");
            }

            var tuserInfo = await _userInfoService.GetRequiredByIdAsync(model.FUserId);
            PresentCheckUserRole(tuserInfo);

            var skuInfo = await (from product in _dbContext.TProduct
                                 join productSku in _dbContext.TProductSku on product.FProductId equals productSku.FProductId
                                 join productSkuPrice in _dbContext.TProductSkuPrice on productSku.FProductSkuId equals productSkuPrice
                                     .FProductSkuId
                                 where product.FRole == tuserInfo.FuserRole.Value &&
                                       product.FActive &&
                                       productSku.FActive &&
                                       productSkuPrice.FCurrencyType == model.FCurrencyType &&
                                       productSku.FProductSkuId == skuId
                                 select new
                                 {
                                     productSku.FProductSkuId,
                                     productSku.FBusiness,
                                     productSkuPrice.FAmount,
                                     productSkuPrice.FSaleUnitPrice,
                                     productSku.FMemberLevel,
                                     productSku.FCode,
                                     productSku.FName,
                                     productSku.FType
                                 }).SingleOrDefaultAsync();

            if (skuInfo == null)
            {
                throw new BusinessException(
                    $"{model.FUserId}对应skuId:{skuId},找不到符合的数据,可能是商品禁用或者货币错误或者SkuId错误或者用户对应商品角色错误");
            }

            var showSkuInfo = GetShowSkuInfo(skuInfo.FName, skuInfo.FType, skuInfo.FBusiness, false);

            var purchaseOrderId = await GetOrderSerialNoAsync();
            var utcNow = DateTime.UtcNow;

            //添加订单记录明细
            var purchaseOrderItem = new TPurchaseOrderItem
            {
                FBusiness = skuInfo.FBusiness,
                FCurrencyType = model.FCurrencyType,
                FPaymentAmount = skuInfo.FAmount * quantity,
                FSaleUnitPrice = skuInfo.FSaleUnitPrice,
                FMemberLevel = skuInfo.FMemberLevel,
                FProductSkuCode = skuInfo.FCode,
                FProductSkuName = showSkuInfo,
                FPurchaseOrderId = purchaseOrderId,
                FQuantity = quantity,
                FUnitPrice = skuInfo.FAmount,
                FCreateAt = utcNow,
                FCreateBy = 0,
                FUpdateAt = utcNow,
                FUpdateBy = 0
            };

            //添加订单记录
            var purchaseOrder = new TPurchaseOrder
            {
                FPurchaseOrderId = purchaseOrderId,
                FUserId = model.FUserId,
                FEmail = tuserInfo.Femail,
                FName = quantity > 1 ? $"{showSkuInfo} × {quantity}" : showSkuInfo,
                FCurrencyType = model.FCurrencyType,
                FSalePrice = purchaseOrderItem.FSaleUnitPrice,
                FPaymentAmount = 0,//purchaseOrderItem.FPaymentAmount, 由于赠送金额所以等于0
                FProviderId = PaymentProvider.Present,
                FServiceType = model.FServiceType,
                FUserPlatformType = UserPlatformType.WEB,
                FStatus = PurchaseOrderStatus.Success,
                FConfirmTime = new DateTime(1970, 1, 1),
                FCreateAt = utcNow,
                FCreateBy = 0,
                FUpdateAt = utcNow,
                FUpdateBy = 0,
                FOriginalOrderId = model.FPurchaseOrderId
            };

            await _dbContext.TPurchaseOrder.AddAsync(purchaseOrder);
            await _dbContext.TPurchaseOrderItem.AddAsync(purchaseOrderItem);

            await _dbContext.SaveChangesAsync();

            return purchaseOrderId;
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        public async Task<long> GetOrderSerialNoAsync()
        {
            const string sqlCmd = @"UPDATE TSerialNo SET FNo = FNo + 1, FUpdateAt = Sysutcdatetime()
                              WHERE FType = 1; 
                              SELECT FNo, FIsProdEnv, FUpdateAt FROM TSerialNo WHERE FType = 1";
            OrderSerialNoDto dto;
            using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
            {
                dto = await connection.QuerySingleOrDefaultAsync<OrderSerialNoDto>(new CommandDefinition(sqlCmd));
            }
            return long.Parse(string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}",
                dto.FUpdateAt.ToString("yyMMddHHmmss", CultureInfo.InvariantCulture),
                dto.EnvNo,
                dto.FNo.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0')));
        }
    }
}
