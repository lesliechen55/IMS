using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.User.Data;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class InvoiceService : IInvoiceService
    {
        private readonly PayDbContext _dbContext;
        private readonly UserDbContext _userDbContext;
        private readonly IMapper _mapper;

        public InvoiceService(PayDbContext dbContext, UserDbContext userDbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _userDbContext = userDbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取发票资料分页列表
        /// </summary>
        /// <param name="input">发票资料列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<InvoicePageDataOutput> outputs, int total)> GetPageDataAsync(InvoicePageDataInput input)
        {
            long? userId = null;
            if (input.UserEmail.IsNotNullOrEmpty())
            {
                userId = await _userDbContext.TuserInfo.Where(w => w.Femail == input.UserEmail).Select(s => s.FuserId).SingleOrDefaultAsync();
            }
            var queryable = _dbContext.TInvoice
                .WhereIf(() => userId.HasValue, w => w.FUserId == userId);
            var count = await queryable.CountAsync();
            var outputs = await queryable
                .OrderByDescending(o => o.FCreateAt)
                .ThenByDescending(x => x.FUpdateAt)
                .ToPage(input.Page, input.Limit)
                .ProjectTo<InvoicePageDataOutput>()
                .ToListAsync();

            var userIds = outputs.Select(s => s.FUserId);
            var user = await _userDbContext.TuserInfo
                .Where(w => userIds.Contains(w.FuserId))
                .Select(s => new { s.FuserId, s.Femail })
                .ToListAsync();

            outputs.ForEach(f => f.FUserEmail = user.SingleOrDefault(s => s.FuserId == f.FUserId)?.Femail);
            return (outputs, count);
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<InvoiceShowOutput> GetByIdAsync(long id)
        {
            var output = await _dbContext.TInvoice.ProjectTo<InvoiceShowOutput>().SingleOrDefaultAsync(x => x.FInvoiceId == id);
            if (null == output)
            {
                throw new BusinessException(nameof(id), id.ToString());
            }
            output.FUserEmail = await _userDbContext.TuserInfo
                .Where(w => w.FuserId == output.FUserId)
                .Select(s => s.Femail)
                .SingleOrDefaultAsync();
            return output;
        }

        /// <summary>
        /// 添加发票资料
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(InvoiceAddInput input, int operatorId)
        {
            if (await _dbContext.TInvoice.AnyAsync(a => a.FUserId == input.FUserId))
            {
                throw new BusinessException($"{nameof(input.FUserEmail)}参数错误,该用户发票资料已存在");
            }
            var model = _mapper.Map<TInvoice>(input);
            model.FInvoiceId = IdHelper.GetGenerateId();
            await _dbContext.TInvoice.AddAsync(model);
            return await _dbContext.SaveChangesAsync(operatorId) > 0;
        }

        /// <summary>
        /// 修改发票资料
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task EditAsync(InvoiceEditInput input, int operatorId)
        {
            var model = await _dbContext.TInvoice.SingleOrDefaultAsync(x => x.FInvoiceId == input.FInvoiceId);
            if (model == null)
            {
                throw new BusinessException(nameof(input.FInvoiceId), input.FInvoiceId.ToString());
            }
            model.FInvoiceType = input.FInvoiceType;
            model.FCompanyName = input.FCompanyName;
            model.FTaxNo = input.FTaxNo;
            model.FTaxPayerCertificateUrl = input.FTaxPayerCertificateUrl;
            model.FBank = input.FBank;
            model.FBankAccount = input.FBankAccount;
            model.FAddress = input.FAddress;
            model.FTelephone = input.FTelephone;

            model.FContact = input.FContact;
            model.FExpressAddress = input.FExpressAddress;
            model.FInvoiceEmail = input.FInvoiceEmail;
            model.FPhone = input.FPhone;
            await _dbContext.SaveChangesAsync(operatorId);
        }
    }
}
