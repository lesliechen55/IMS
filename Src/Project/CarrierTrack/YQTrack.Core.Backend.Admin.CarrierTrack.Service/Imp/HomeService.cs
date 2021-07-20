using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.CarrierTrack.Core;
using YQTrack.Core.Backend.Admin.CarrierTrack.Data;
using YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Input;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Output;
using YQTrack.Core.Backend.Admin.CommonService;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.Service;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Service.Imp
{
    public class HomeService : BaseCarrierTrackService, IHomeService
    {
        private readonly CarrierTrackDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUserInfoService _userInfoService;

        public HomeService(CarrierTrackDbContext dbContext, IMapper mapper, IUserInfoService userInfoService, IUserService userService) : base(dbContext, userService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userInfoService = userInfoService;
        }

        public async Task<IEnumerable<IndexPageDataOutput>> GetPageDataAsync(IndexPageDataInput input)
        {
            //if (input.Email.IsNullOrWhiteSpace())
            //{
            //    throw new BusinessException($"{nameof(input.Email)}参数为空错误");
            //}
            var userId = await _userInfoService.GetUserIdByEmailAsync(input.Email);
            if (userId.HasValue)
            {
                await SetDbRouteAsync(userId.Value);
                var queryable = _dbContext.TControl.Where(x => x.FUserId == userId.Value)
                    .WhereIf(() => input.Enable.HasValue, x => x.FEnable == input.Enable.Value)
                    .WhereIf(() => input.OfflineDay.HasValue, x => x.FLastAccessTime.HasValue && EF.Functions.DateDiffDay(x.FLastAccessTime.Value, DateTime.UtcNow) >= input.OfflineDay.Value);
                var outputs = await queryable.OrderByDescending(x => x.FUpdateTime).ProjectTo<IndexPageDataOutput>().ToListAsync();
                return outputs;
            }

            var outputList = new List<IndexPageDataOutput>();
            var allDbConnections = DbConnectionTool.GetAllDbConnections();
            foreach (var dbConnection in allDbConnections)
            {
                if (_dbContext.Database.GetDbConnection().ConnectionString != dbConnection)
                {
                    _dbContext.Database.GetDbConnection().ConnectionString = dbConnection;
                }
                var outputs = await _dbContext.TControl
                    .WhereIf(() => input.Enable.HasValue, x => x.FEnable == input.Enable.Value)
                    .WhereIf(() => input.OfflineDay.HasValue, x => x.FLastAccessTime.HasValue && EF.Functions.DateDiffDay(x.FLastAccessTime.Value, DateTime.UtcNow) >= input.OfflineDay.Value)
                    .OrderByDescending(x => x.FUpdateTime)
                    .ProjectTo<IndexPageDataOutput>()
                    .ToListAsync();
                outputList.AddRange(outputs);
            }
            outputList = outputList.OrderByDescending(x => x.FUpdateTime).ToList();
            return outputList;
        }

        public async Task AddAsync(CarrierTrackUserAddInput input, int operatorId)
        {
            var userId = await _userInfoService.GetUserIdByEmailAsync(input.FEmail);
            if (!userId.HasValue) throw new BusinessException(nameof(input.FEmail), input.FEmail);

            var info = await _userInfoService.GetRequiredByIdAsync(userId.Value);
            if (!info.FuserRole.HasValue)
            {
                throw new BusinessException($"{info.Femail}不包含任何角色不能添加");
            }
            if (info.FuserRole.Value != UserRoleType.Carrier)
            {
                throw new BusinessException($"{info.Femail}的角色:{info.FuserRole.Value.ToString()}不是:{UserRoleType.Carrier.ToString()},不能添加");
            }
            await SetDbRouteAsync(userId.Value);
            if (await _dbContext.TControl.AnyAsync(x => x.FUserId == userId.Value))
            {
                throw new BusinessException($"当前用户:{input.FEmail}已经存在,不能重复添加");
            }
            var control = _mapper.Map<TControl>(input);
            control.FUserId = userId.Value;
            control.FCreateBy = operatorId;
            control.FControlId = IdHelper.GetGenerateId();
            await _dbContext.TControl.AddAsync(control);
            _dbContext.SaveChanges();
        }

        public async Task<(IndexPageDataOutput output, int availableTrackNum, int buyTotal)> GetByIdAsync(long controlId, long userId)
        {
            var control = await GetRequiredByIdAsync(controlId, userId);
            var output = _mapper.Map<IndexPageDataOutput>(control);

            var availableTrackNum = await _dbContext.TBusinessCtrl.Where(x =>
                x.FAvailable &&
                EF.Functions.DateDiffDay(x.FStartTime, DateTime.UtcNow) >= 0 &&
                x.FStopTime.HasValue && x.FStopTime.Value > DateTime.UtcNow &&
                x.FRemainCount > 0 &&
                x.FBusinessCtrlType == (short)BusinessCtrlType.CarrierTrack
                && x.FUserId == userId).Select(x => x.FRemainCount).SumAsync() ?? 0;

            var buyTotal = await _dbContext.TBusinessCtrl.Where(x =>
                x.FBusinessCtrlType == (short)BusinessCtrlType.CarrierTrack &&
                x.FPurchaseOrderId > 0 &&
                x.FProviderId.HasValue && x.FProviderId.Value != (int)PaymentProvider.Present && x.FProviderId.Value != (int)PaymentProvider.Unknown &&
                x.FUserId == userId).Select(x => x.FServiceCount).SumAsync();

            return (output, availableTrackNum, buyTotal);
        }

        public async Task EditAsync(long requestId, long userId, int requestImportTodayLimit, int requestExportTimeLimit, bool requestEnable, int loginManagerId)
        {
            var control = await GetRequiredByIdAsync(requestId, userId);
            control.FImportTodayLimit = requestImportTodayLimit;
            control.FExportTimeLimit = requestExportTimeLimit;
            control.FEnable = requestEnable;
            control.FUpdateBy = loginManagerId;
            control.FUpdateTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
    }
}