
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.User.Data;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class InvoiceApplyService : IInvoiceApplyService
    {
        private readonly PayDbContext _dbContext;
        private readonly UserDbContext _userDbContext;
        private readonly IMapper _mapper;

        public InvoiceApplyService(PayDbContext dbContext, UserDbContext userDbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _userDbContext = userDbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取发票申请分页列表
        /// </summary>
        /// <param name="input">发票申请列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<InvoiceApplyPageDataOutput> outputs, int total)> GetPageDataAsync(InvoiceApplyPageDataInput input)
        {
            long? userId = null;
            if (input.UserEmail.IsNotNullOrEmpty())
            {
                userId = await _userDbContext.TuserInfo.Where(w => w.Femail == input.UserEmail).Select(s => s.FuserId).SingleOrDefaultAsync();
            }
            var queryable = _dbContext.TInvoiceApply
                .OrderByDescending(o => o.FCreateAt)
                .ThenByDescending(x => x.FUpdateAt)
                .Join(_dbContext.TInvoiceApplyExtra, x => x.FInvoiceApplyId, y => y.FInvoiceApplyId, (x, y) => new InvoiceApplyPageDataOutput
                {
                    FInvoiceApplyId = x.FInvoiceApplyId,
                    FAmount = x.FAmount,
                    FCompanyName = y.FCompanyName,
                    FCreateAt = x.FCreateAt,
                    FCurrencyType = x.FCurrencyType,
                    FHandleTime = x.FHandleTime,
                    FInvoiceType = x.FInvoiceType,
                    FRejectReason = x.FRejectReason,
                    FSendInfo = x.FSendInfo,
                    FSendType = x.FSendType,
                    FStatus = x.FStatus,
                    FUserId = x.FUserId
                })
                .WhereIf(() => userId.HasValue, w => w.FUserId == userId)
                .WhereIf(() => input.Status.HasValue, w => w.FStatus == input.Status);
            var count = await queryable.CountAsync();
            var outputs = await queryable
                .ToPage(input.Page, input.Limit)
                .ToListAsync();

            var userIds = outputs.Select(s => s.FUserId);
            var user = await _userDbContext.TuserInfo
                .Where(w => userIds.Contains(w.FuserId))
                .Select(s => new { s.FuserId, s.Femail })
                .ToListAsync();
            outputs.ForEach(f => f.FEmail = user.SingleOrDefault(s => s.FuserId == f.FUserId)?.Femail);
            return (outputs, count);
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InvoiceApplyShowOutput> GetByIdAsync(long id)
        {
            var output = await _dbContext.TInvoiceApply.Where(w => w.FInvoiceApplyId == id)
                .Join(_dbContext.TInvoiceApplyExtra, a => a.FInvoiceApplyId, b => b.FInvoiceApplyId, (a, b) => new { InvoiceApply = a, InvoiceApplyExtra = b })
                .Select(s => new InvoiceApplyShowOutput
                {
                    FInvoiceApplyId = s.InvoiceApply.FInvoiceApplyId,
                    FInvoiceType = s.InvoiceApply.FInvoiceType,
                    FUserId = s.InvoiceApply.FUserId,
                    FCreateAt = s.InvoiceApply.FCreateAt,
                    FCurrencyType = s.InvoiceApply.FCurrencyType,
                    FAmount = s.InvoiceApply.FAmount,
                    FStatus = s.InvoiceApply.FStatus,
                    FRejectReason = s.InvoiceApply.FRejectReason,
                    FRemark = s.InvoiceApply.FRemark,
                    FSendInfo = s.InvoiceApply.FSendInfo,
                    FSendType = s.InvoiceApply.FSendType,
                    FCompanyName = s.InvoiceApplyExtra.FCompanyName,
                    FTaxNo = s.InvoiceApplyExtra.FTaxNo,
                    FTaxPayerCertificateUrl = s.InvoiceApplyExtra.FTaxPayerCertificateUrl,
                    FBank = s.InvoiceApplyExtra.FBank,
                    FBankAccount = s.InvoiceApplyExtra.FBankAccount,
                    FPhone = s.InvoiceApplyExtra.FPhone,
                    FContact = s.InvoiceApplyExtra.FContact,
                    FTelephone = s.InvoiceApplyExtra.FTelephone,
                    FAddress = s.InvoiceApplyExtra.FAddress,
                    FExpressAddress = s.InvoiceApplyExtra.FExpressAddress,
                    FInvoiceEmail = s.InvoiceApplyExtra.FInvoiceEmail
                }).SingleOrDefaultAsync();
            if (null == output)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(output)}数据不存在");
            }
            output.FEmail = await _userDbContext.TuserInfo
                .Where(w => w.FuserId == output.FUserId)
                .Select(s => s.Femail)
                .SingleOrDefaultAsync();
            output.InvoicePaymentList = await _dbContext.TInvoiceApplyPayment
                .Where(w => w.FInvoiceApplyId == output.FInvoiceApplyId)
                .ProjectTo<InvoicePaymentOutput>()
                .ToListAsync();
            // 赋值发票支付数据的支付类型
            var orderIds = output.InvoicePaymentList.Select(x => x.FOrderId).ToArray();
            var list = await _dbContext.TPurchaseOrder.Where(x => orderIds.Contains(x.FPurchaseOrderId)).Select(x => new { x.FPurchaseOrderId, x.FProviderId }).ToListAsync();
            output.InvoicePaymentList.ForEach(x =>
            {
                var temp = list.FirstOrDefault(c => c.FPurchaseOrderId == x.FOrderId);
                if (temp != null)
                {
                    x.PaymentProvider = temp.FProviderId;
                }
            });
            return output;
        }

        /// <summary>
        /// 发票申请审核通过
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<long> PassAsync(InvoiceApplyPassInput input, int operatorId)
        {
            var model = await _dbContext.TInvoiceApply.SingleOrDefaultAsync(x => x.FInvoiceApplyId == input.FInvoiceApplyId);
            if (model == null)
            {
                throw new BusinessException($"{nameof(input.FInvoiceApplyId)}参数错误,{nameof(model)}数据不存在");
            }
            if (model.FStatus != InvoiceAuditStatus.Awaiting)
            {
                throw new BusinessException($"该数据已被审核，请刷新");
            }
            model.FStatus = InvoiceAuditStatus.Approved;
            model.FSendType = input.FSendType;
            model.FSendInfo = input.FSendInfo;
            model.FRemark = input.FRemark;
            model.FHandleTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync(operatorId);
            return model.FUserId;
        }

        /// <summary>
        /// 发票申请驳回
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<long> RejectAsync(InvoiceApplyRejectInput input, int operatorId)
        {
            var model = await _dbContext.TInvoiceApply.SingleOrDefaultAsync(x => x.FInvoiceApplyId == input.FInvoiceApplyId);
            if (model == null)
            {
                throw new BusinessException($"{nameof(input.FInvoiceApplyId)}参数错误,{nameof(model)}数据不存在");
            }
            if (model.FStatus != InvoiceAuditStatus.Awaiting)
            {
                throw new BusinessException($"该数据已被审核，请刷新");
            }
            model.FStatus = InvoiceAuditStatus.Rejected;
            model.FRejectReason = input.FRejectReason;
            model.FRemark = input.FRemark;
            model.FHandleTime = DateTime.UtcNow;

            //更新交易记录
            var paymentIdList = await _dbContext.TInvoiceApplyPayment
                .Where(w => w.FInvoiceApplyId == model.FInvoiceApplyId)
                .Select(s => s.FPaymentId)
                .ToListAsync();
            var paymentList = await _dbContext.TPayment.Where(w => paymentIdList.Contains(w.FPaymentId)).ToListAsync();
            paymentList.ForEach(f => f.FApplyInvoice = false);
            await _dbContext.SaveChangesAsync(operatorId);
            return model.FUserId;
        }
    }
}
