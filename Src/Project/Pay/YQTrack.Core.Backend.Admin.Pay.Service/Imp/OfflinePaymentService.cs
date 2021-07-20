using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.User.Data;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class OfflinePaymentService : IOfflinePaymentService
    {
        private readonly PayDbContext _dbContext;
        private readonly UserDbContext _userDbContext;
        private readonly IPurchaseOrderService _purchaseOrderService;

        public OfflinePaymentService(PayDbContext dbContext, UserDbContext userDbContext, IPurchaseOrderService purchaseOrderService)
        {
            _dbContext = dbContext;
            _userDbContext = userDbContext;
            _purchaseOrderService = purchaseOrderService;
        }

        /// <summary>
        /// 获取线下交易分页列表
        /// </summary>
        /// <param name="input">线下交易列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<OfflinePaymentPageDataOutput> outputs, int total)> GetPageDataAsync(OfflinePaymentPageDataInput input)
        {
            long? userId = null;
            if (input.Email.IsNotNullOrEmpty())
            {
                userId = await _userDbContext.TuserInfo.Where(w => w.Femail == input.Email).Select(s => s.FuserId).SingleOrDefaultAsync();
            }
            var queryable = _dbContext.TOfflinePayment
                .WhereIf(() => userId.HasValue, w => w.FUserId == userId)
                .WhereIf(() => input.Status.HasValue, w => w.FStatus == input.Status);
            var count = await queryable.CountAsync();
            var outputs = await queryable
                .OrderByDescending(o => o.FCreateAt)
                .ThenByDescending(x => x.FUpdateAt)
                .ToPage(input.Page, input.Limit)
                .ProjectTo<OfflinePaymentPageDataOutput>()
                .ToListAsync();
            var userIds = outputs.Select(s => s.FUserId);
            var user = await _userDbContext.TuserInfo
                .Where(w => userIds.Contains(w.FuserId))
                .Select(s => new { s.FuserId, s.Femail })
                .ToListAsync();

            var purchaseOrderIdList = await _dbContext.TOfflinePaymentOrder
                .Where(w => outputs.Select(s => s.FOfflinePaymentId).Contains(w.FOfflinePaymentId) && w.FPurchaseOrderId.HasValue)
                .Select(s => new
                {
                    FPurchaseOrderId = s.FPurchaseOrderId.Value,
                    s.FOfflinePaymentId
                })
                .ToListAsync();
            outputs.ForEach(f =>
            {
                f.FEmail = user.SingleOrDefault(s => s.FuserId == f.FUserId)?.Femail;
                f.PurchaseOrderIdList = purchaseOrderIdList
                .Where(w => w.FOfflinePaymentId == f.FOfflinePaymentId)
                .Select(s => s.FPurchaseOrderId).ToList();
            });
            return (outputs, count);
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<OfflinePaymentShowOutput> GetByIdAsync(long id)
        {
            var output = await _dbContext.TOfflinePayment.Where(w => w.FOfflinePaymentId == id).ProjectTo<OfflinePaymentShowOutput>().SingleOrDefaultAsync();
            if (null == output)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(output)}数据不存在");
            }
            output.FEmail = await _userDbContext.TuserInfo
                .Where(w => w.FuserId == output.FUserId)
                .Select(s => s.Femail)
                .SingleOrDefaultAsync();
            output.PaymentList = await _dbContext.TOfflinePaymentOrder
                .Where(w => w.FOfflinePaymentId == output.FOfflinePaymentId)
                .ProjectTo<OfflinePaymentOrderOutput>()
                .ToListAsync();
            return output;
        }

        /// <summary>
        /// 线下交易审核通过
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<long> PassAsync(OfflinePaymentPassInput input, int operatorId)
        {
            var model = await _dbContext.TOfflinePayment.SingleOrDefaultAsync(x => x.FOfflinePaymentId == input.FOfflinePaymentId);
            if (model == null)
            {
                throw new BusinessException($"{nameof(input.FOfflinePaymentId)}参数错误,{nameof(model)}数据不存在");
            }
            if (model.FStatus != OfflineAuditStatus.Awaiting)
            {
                throw new BusinessException($"该数据已被审核，请刷新");
            }
            model.FStatus = OfflineAuditStatus.Approved;
            model.FRemark = input.FRemark;
            model.FHandleTime = DateTime.UtcNow;

            var offlineOrderList = await _dbContext.TOfflinePaymentOrder.Where(w => w.FOfflinePaymentId == model.FOfflinePaymentId).ToListAsync();
            if (!offlineOrderList.Any())
            {
                throw new BusinessException("订单数据不存在");
            }
            //查询商品及价格信息
            var skuIds = offlineOrderList.Select(s => s.FProductSkuId);
            var productSkuPriceOutputList = await _dbContext.TProductSku
               .Where(w => skuIds.Contains(w.FProductSkuId) && w.FActive)
               .Join(_dbContext.TProductSkuPrice.Where(w => w.FCurrencyType == model.FCurrencyType), x => x.FProductSkuId, y => y.FProductSkuId, (x, y) => new { ProductSku = x, ProductSkuPrice = y })
               .Join(_dbContext.TProduct.Where(w => w.FActive), x => x.ProductSku.FProductId, y => y.FProductId, (x, y) => new { x.ProductSku, x.ProductSkuPrice, Product = y })
               .Select(s => new OfflineProductSkuPriceOutput
               {
                   FProductSkuId = s.ProductSku.FProductSkuId,
                   FCode = s.ProductSku.FCode,
                   FName = s.ProductSku.FName,
                   FMemberLevel = s.ProductSku.FMemberLevel,
                   FServiceType = s.Product.FServiceType,
                   FBusiness = s.ProductSku.FBusiness,
                   FAmount = s.ProductSkuPrice.FAmount,
                   FSaleUnitPrice = s.ProductSkuPrice.FSaleUnitPrice,
                   FCurrencyType = s.ProductSkuPrice.FCurrencyType,
                   FPlatformType = s.ProductSkuPrice.FPlatformType
               }).ToListAsync();
            if (!productSkuPriceOutputList.Any())
            {
                throw new BusinessException("商品相关数据不存在");
            }
            var userEmail = await _userDbContext.TuserInfo
                .Where(w => w.FuserId == model.FUserId)
                .Select(s => s.Femail)
                .SingleOrDefaultAsync();
            var utcNow = DateTime.UtcNow;
            offlineOrderList.ForEach(f =>
            {
                //查询商品及价格信息
                var productSkuPriceOutput = productSkuPriceOutputList.SingleOrDefault(s => s.FProductSkuId == f.FProductSkuId);
                if (productSkuPriceOutput == null)
                {
                    throw new BusinessException("商品相关数据不存在");
                }
                var purchaseOrderId = _purchaseOrderService.GetOrderSerialNoAsync().Result;

                //添加订单记录明细
                var purchaseOrderItem = new TPurchaseOrderItem
                {
                    FBusiness = productSkuPriceOutput.FBusiness,
                    FCurrencyType = model.FCurrencyType,
                    FPaymentAmount = productSkuPriceOutput.FAmount * f.FQuantity,
                    FSaleUnitPrice = productSkuPriceOutput.FSaleUnitPrice,
                    FMemberLevel = productSkuPriceOutput.FMemberLevel,
                    FProductSkuCode = productSkuPriceOutput.FCode,
                    FProductSkuName = f.FProductSkuName,
                    FPurchaseOrderId = purchaseOrderId,
                    FQuantity = f.FQuantity,
                    FUnitPrice = productSkuPriceOutput.FAmount,
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
                    FEmail = userEmail,
                    FName = f.FQuantity > 1 ? $"{f.FProductSkuName} × {f.FQuantity}" : f.FProductSkuName,
                    FCurrencyType = model.FCurrencyType,
                    FSalePrice = purchaseOrderItem.FSaleUnitPrice,
                    FPaymentAmount = purchaseOrderItem.FPaymentAmount,
                    FProviderId = model.FProviderId,
                    FServiceType = productSkuPriceOutput.FServiceType,
                    FUserPlatformType = UserPlatformType.WEB,
                    FStatus = PurchaseOrderStatus.Success,
                    FConfirmTime = new DateTime(1970, 1, 1),
                    FCreateAt = utcNow,
                    FCreateBy = 0,
                    FUpdateAt = utcNow,
                    FUpdateBy = 0
                };

                //添加交易记录
                var payment = new TPayment
                {
                    FPayerId = purchaseOrder.FUserId,
                    FOrderId = purchaseOrderId,
                    FOrderName = purchaseOrder.FName,
                    FCurrencyType = purchaseOrder.FCurrencyType,
                    FPaymentAmount = purchaseOrder.FPaymentAmount,
                    FPaymentStatus = PaymentStatus.Success,
                    FPlatformType = purchaseOrder.FUserPlatformType,
                    FProviderId = purchaseOrder.FProviderId,
                    // 注释:因为支付表针对此字段有比较多的依赖,不方便调整字段长度,且线下交易流水号不能绝对认为是支付数据的交易号
                    //FProviderTradeNo = model.FTransferNo,
                    FServiceType = purchaseOrder.FServiceType,
                    FApplyInvoice = false,
                    FCreateAt = utcNow,
                    FCreateBy = 0,
                    FUpdateAt = utcNow,
                    FUpdateBy = 0
                };

                //修改线下交易订单，回填订单号
                f.FEffectiveTime = utcNow;
                f.FPurchaseOrderId = purchaseOrderId;

                _dbContext.TPurchaseOrder.Add(purchaseOrder);
                _dbContext.TPurchaseOrderItem.Add(purchaseOrderItem);
                _dbContext.TPayment.Add(payment);
            });

            await _dbContext.SaveChangesAsync(operatorId);
            return model.FUserId;
        }

        /// <summary>
        /// 线下交易驳回
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<long> RejectAsync(OfflinePaymentRejectInput input, int operatorId)
        {
            var model = await _dbContext.TOfflinePayment.SingleOrDefaultAsync(x => x.FOfflinePaymentId == input.FOfflinePaymentId);
            if (model == null)
            {
                throw new BusinessException($"{nameof(input.FOfflinePaymentId)}参数错误,{nameof(model)}数据不存在");
            }
            if (model.FStatus != OfflineAuditStatus.Awaiting)
            {
                throw new BusinessException($"该数据已被审核，请刷新");
            }
            model.FStatus = OfflineAuditStatus.Rejected;
            model.FRejectReason = input.FRejectReason;
            model.FRemark = input.FRemark;
            model.FHandleTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(operatorId);
            return model.FUserId;
        }
    }
}
