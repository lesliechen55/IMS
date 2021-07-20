using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YQTrack.Backend.Models;
using YQTrack.Backend.Models.Enums;
using YQTrack.Backend.Sharding;
using YQTrack.Core.Backend.Admin.CarrierTrack.Data;
using YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.Service;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Service.Imp
{
    public abstract class BaseCarrierTrackService
    {
        private readonly CarrierTrackDbContext _dbContext;
        private readonly IUserService _userService;

        protected BaseCarrierTrackService(CarrierTrackDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        /// <summary>
        /// 根据用户路由动态切换数据库上下文的数据库连接字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected virtual async Task SetDbRouteAsync(long userId)
        {
            var userRoute = await _userService.GetUserDataRouteAsync(userId);
            if (userRoute == null)
            {
                throw new BusinessException($"当前用户:{userId}找不到路由信息错误");
            }
            if (userRoute.FdbNo == 0)
            {
                throw new BusinessException($"当前用户:{userId}路由信息错误,详情:{nameof(userRoute.FdbNo)}:{userRoute.FdbNo}");
            }
            var dataRouteModel = new DataRouteModel
            {
                // ReSharper disable once PossibleInvalidOperationException
                NodeId = userRoute.FnodeId.Value,
                // ReSharper disable once PossibleInvalidOperationException
                DbNo = userRoute.FdbNo.Value,
                // ReSharper disable once PossibleInvalidOperationException
                TableNo = userRoute.FtableNo.Value,
                // ReSharper disable once PossibleInvalidOperationException
                UserRole = (byte)userRoute.FuserRole.Value,
                IsArchived = false,
                IsWrite = true
            };
            var connectionString = DBShardingRouteFactory.GetDBConnStr(YQDbType.CarrierTrack.ToString(), dataRouteModel);
            if (!string.IsNullOrWhiteSpace(connectionString) && _dbContext.Database.GetDbConnection().ConnectionString != connectionString)
            {
                _dbContext.Database.GetDbConnection().ConnectionString = connectionString;
            }
        }

        protected virtual async Task<TControl> GetRequiredByIdAsync(long id, long userId)
        {
            await SetDbRouteAsync(userId);
            var control = await _dbContext.TControl.SingleOrDefaultAsync(x => x.FControlId == id && x.FUserId == userId);
            if (control == null) throw new BusinessException(nameof(id), id.ToString());
            return control;
        }
    }
}