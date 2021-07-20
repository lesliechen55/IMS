using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Models;
using YQTrack.Backend.Models.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Backend.Sharding;
using YQTrack.Backend.Sharding.Router;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Sharding;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.Seller.Data;
using YQTrack.Core.Backend.Admin.Seller.Data.Models;
using YQTrack.Core.Backend.Admin.Seller.Service;
using YQTrack.Core.Backend.Admin.User.Data;
using YQTrack.Core.Backend.Admin.User.Data.Models;
using YQTrack.Core.Backend.Admin.User.DTO.Input;
using YQTrack.Core.Backend.Admin.User.DTO.Output;
using YQTrack.Core.Backend.Enums.Pay;
using YQTrack.SRVI.DeleteUser;

namespace YQTrack.Core.Backend.Admin.User.Service.Imp
{
    public class UserService : IUserService
    {
        private readonly IDataAccessor<SellerOrderDBContext> _sellerOrderDataAccessor;
        private readonly IDataAccessor<SellerMessageDBContext> _sellerMessageDataAccessor;
        private readonly ITableMappable _tableMapper;
        private readonly UserDbContext _userDbContext;
        private readonly PayDbContext _payDbContext;
        private readonly IDeleteUserRpcService _deleteUserRpcService;
        private readonly IMapper _mapper;

        public UserService(IDataAccessor<SellerOrderDBContext> sellerOrderDataAccessor, IDataAccessor<SellerMessageDBContext> sellerMessageDataAccessor, SellerShardingMapper tableMapper, UserDbContext userDbContext, PayDbContext payDbContext, IDeleteUserRpcService deleteUserRpcService, IMapper mapper)
        {
            _sellerOrderDataAccessor = sellerOrderDataAccessor;
            _sellerMessageDataAccessor = sellerMessageDataAccessor;
            _tableMapper = tableMapper;
            _userDbContext = userDbContext;
            _payDbContext = payDbContext;
            _deleteUserRpcService = deleteUserRpcService;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<UserPageDataOutput> outputs, int total)> GetUserPageDataAsync(UserPageDataInput input)
        {
            var queryable = _userDbContext.TuserInfo
                .WhereIf(() => input.UserId.HasValue, x => x.FuserId == input.UserId.Value)
                .WhereIf(() => !string.IsNullOrWhiteSpace(input.Email), x => x.Femail == input.Email)
                .Join(_userDbContext.TuserProfile, x => x.FuserId, y => y.FuserId, (x, y) => new
                {
                    x.FuserId,
                    x.FuserRole,
                    x.FnodeId,
                    x.FdbNo,
                    x.FnickName,
                    x.Femail,
                    x.Fstate,
                    x.FlastSignIn,
                    x.FcreateTime,
                    x.FcreateBy,
                    x.FupdateTime,
                    x.FupdateBy,
                    y.Flanguage,
                    y.Fcountry,
                    y.FisPay,
                    y.Fsource,
                }).ProjectTo<UserPageDataOutput>();

            var count = await queryable.CountAsync();

            var outputs = await queryable.OrderByDescending(x => x.FcreateTime).ToPage(input.Page, input.Limit).ToListAsync();

            return (outputs, count);
        }

        public async Task<(IEnumerable<UserFeedbackPageDataOutput> outputs, int total)> GetUserFeedbackPageDataAsync(UserFeedbackPageDataInput input)
        {
            var queryable = _userDbContext.TuserFeedback.WhereIf(() => input.UserId.HasValue && input.UserId.Value > 0,
                    x => x.FuserId == input.UserId.Value)
                .WhereIf(() => !input.Email.IsNullOrWhiteSpace(), x => x.Femail == input.Email.Trim())
                .WhereIf(() => !input.Content.IsNullOrWhiteSpace(), x => x.Ffeedback.Contains(input.Content.Trim()));

            var count = await queryable.CountAsync();

            var outputs = await queryable
                .OrderBy(x => x.Fstate)
                .ThenByDescending(x => x.FupdateTime)
                .ToPage(input.Page, input.Limit)
                .ProjectTo<UserFeedbackPageDataOutput>().ToListAsync();

            return (outputs, count);
        }

        public async Task Reply(string feedbackIds, string content, int updateBy)
        {
            if (feedbackIds.IsNullOrWhiteSpace())
                throw new BusinessException($"参数{nameof(feedbackIds)}:{feedbackIds}错误,不能为空");
            IEnumerable<long> ids = feedbackIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => long.Parse(s));
            List<TuserFeedback> list = await _userDbContext.TuserFeedback.Where(x => ids.Contains(x.FfeedbackId)).ToListAsync();
            if (list == null || !list.Any())
            {
                throw new BusinessException($"参数{nameof(feedbackIds)}:{feedbackIds}错误,找不到数据");
            }
            list.ForEach(feedback =>
            {
                feedback.Fstate = (byte)Enums.User.UserFeedbackState.Handled;
                feedback.FreplyContent = content;
                feedback.FreplyTime = DateTime.UtcNow;
                feedback.FreplyUserId = updateBy;
            });

            await _userDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// 更换分库/分表
        /// </summary>
        /// <returns></returns>
        private (DataRouteModel dataRouteModel, string connectionString, List<TableMappingRule> rules) SetDBRoute(UserDetailOutput output, YQDbType dbType, params Type[] mappingTypes)
        {
            //更换分库/分表
            var dataRouteModel = new DataRouteModel()
            {
                NodeId = output.FnodeId.Value,
                DbNo = output.FdbNo.Value,
                TableNo = output.FtableNo.Value,
                UserRole = (byte)output.FuserRole.Value,
                IsArchived = false,
                IsWrite = true
            };

            DBRoute dbRoute = DBShardingRouteFactory.GetDBRules(dbType.ToString(), dataRouteModel.NodeId).FirstOrDefault(f => f.Value.DBNo == dataRouteModel.DbNo && f.Value.IsArchived == dataRouteModel.IsArchived).Value;
            var connectionString = dbRoute?.ConnStrs?.FirstOrDefault(f => f.IsMaster == dataRouteModel.IsWrite)?.ConnStr;

            List<TableMappingRule> rules = new List<TableMappingRule>();
            foreach (var mappingType in mappingTypes)
            {
                rules.Add(new TableMappingRule
                {
                    MappingType = mappingType,
                    Mapper = _tableMapper,
                    Condition = dbRoute.IsSubmeter ? output.FtableNo : null
                });
            }
            return (dataRouteModel, connectionString, rules);
        }

        public async Task<UserDetailOutput> GetDetailAsync(long userId)
        {
            var output = await _userDbContext.TuserInfo.AsNoTracking().Where(w => w.FuserId == userId)
                    .Join(_userDbContext.TuserProfile, x => x.FuserId, y => y.FuserId
                    , (x, y) => new
                    {
                        x.FuserId,
                        x.Femail,
                        x.FnickName,
                        x.FnodeId,
                        x.FdbNo,
                        x.FtableNo,
                        y.Fsource,
                        x.FuserRole,
                        x.FlastSignIn,
                        x.FcreateTime
                    }
                    ).ProjectTo<UserDetailOutput>().SingleOrDefaultAsync();

            if (output == null)
            {
                throw new BusinessException($"参数{nameof(userId)}:{userId}错误,找不到数据");
            }
            if (output.FuserRole == UserRoleType.Buyer)
            {
                output.ListMemberInfo = _userDbContext.TuserMemberInfo.AsNoTracking()
                    .Where(w => w.FuserId == userId)
                    .OrderByDescending(o => o.FstartTime)
                    .ProjectTo<MemberInfoOutput>()
                    .ToList();
                output.ListUserDevice = _userDbContext.TuserDevice.AsNoTracking()
                        .Where(w => w.FuserId == userId)
                        .OrderByDescending(o => o.FlastVisitTime)
                        .ProjectTo<UserDeviceOutput>()
                        .ToList();
            }
            else if (output.FuserRole == UserRoleType.Seller)
            {
                //获取跟踪额度
                var (dataRouteModel, connectionString, rules) = SetDBRoute(output, YQDbType.Seller, typeof(TBusinessCtrl));
                SellerOrderDBContext.ConnectString = connectionString;
                _sellerOrderDataAccessor.ChangeDataBase(connectionString, rules);

                output.SellerInfo = await _sellerOrderDataAccessor.GetQueryable<TBusinessCtrl>()
                    .Where(w => w.FUserId == output.FuserId && w.FAvailable == true && w.FBusinessCtrlType == (short)BusinessCtrlType.SellerTrack && w.FStartTime <= DateTime.UtcNow && w.FStopTime.Value > DateTime.UtcNow)
                    .GroupBy(g => g.FUserId)
                    .Select(s => new SellerInfoOutput
                    {
                        TrackServiceCount = s.Sum(sum => sum.FServiceCount),
                        TrackRemainCount = s.Sum(sum => sum.FRemainCount.Value)
                    }).FirstOrDefaultAsync() ?? new SellerInfoOutput();
                output.SellerInfo.UserRoute = new DTO.UserRouteDto { UserId = output.FuserId, Email = output.Femail, NickName = output.FnickName, DataRouteModel = dataRouteModel };

                //获取邮件额度
                var (msgDataRouteModel, msgConnectionString, msgRules) = SetDBRoute(output, YQDbType.SellerMessage, typeof(TMsgEmailBusinessCtrl));
                SellerMessageDBContext.ConnectString = msgConnectionString;
                _sellerMessageDataAccessor.ChangeDataBase(msgConnectionString, msgRules);
                var emailQuota = await _sellerMessageDataAccessor.GetQueryable<TMsgEmailBusinessCtrl>()
                    .Where(w => w.FUserId == output.FuserId && w.FAvailable == true && w.FBusinessCtrlType == (short)BusinessCtrlType.SellerEmail && w.FStartTime <= DateTime.UtcNow && w.FStopTime > DateTime.UtcNow)
                    .GroupBy(g => g.FUserId)
                    .Select(s => new
                    {
                        EmailServiceCount = s.Sum(sum => sum.FServiceCount),
                        EmailRemainCount = s.Sum(sum => sum.FRemainCount.Value)
                    }).FirstOrDefaultAsync();
                if (emailQuota != null)
                {
                    output.SellerInfo.EmailServiceCount = emailQuota.EmailServiceCount;
                    output.SellerInfo.EmailRemainCount = emailQuota.EmailRemainCount;
                }
            }
            output.ListPayment = await _payDbContext.TPayment
                .Where(w => w.FPayerId == userId && (w.FPaymentStatus == PaymentStatus.Success || w.FPaymentStatus == PaymentStatus.Refunded))
                .ProjectTo<PaymentOutput>()
                .OrderByDescending(o => o.FUpdateAt)
                .Take(10)
                .ToListAsync();
            return output;
        }

        /// <summary>
        /// 根据ID获取用户基本信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserBaseInfoOutput> GetUserBaseInfoAsync(long userId)
        {
            return await _userDbContext.TuserInfo
                .Join(_userDbContext.TuserProfile, x => x.FuserId, y => y.FuserId, (x, y) => new UserBaseInfoOutput
                {
                    FUserId = x.FuserId,
                    FNickName = x.FnickName,
                    FUserRole = x.FuserRole,
                    FEmail = x.Femail,
                    FLanguage = y.Flanguage
                })
                .Where(w => w.FUserId == userId)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// 根据邮件获取用户基本信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<UserBaseInfoOutput> GetUserBaseInfoByEmailAsync(string email)
        {
            return await _userDbContext.TuserInfo
                 .Join(_userDbContext.TuserProfile, x => x.FuserId, y => y.FuserId, (x, y) => new UserBaseInfoOutput
                 {
                     FUserId = x.FuserId,
                     FNickName = x.FnickName,
                     FUserRole = x.FuserRole,
                     FEmail = x.Femail,
                     FLanguage = y.Flanguage
                 })
                .Where(w => w.FEmail == email)
                .SingleOrDefaultAsync();
        }

        /// <summary>
        /// 获取用户数据库路由信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserDataRouteOutput> GetUserDataRouteAsync(long userId)
        {
            return await _userDbContext.TuserInfo
                .Where(w => w.FuserId == userId)
                .ProjectTo<UserDataRouteOutput>()
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDataRoutePlusOutput>> GetUserDataRouteListAsync(long[] userIdList)
        {
            if (userIdList == null || !userIdList.Any()) return new List<UserDataRoutePlusOutput>();
            return await _userDbContext.TuserInfo
                .Where(w => userIdList.Contains(w.FuserId))
                .ProjectTo<UserDataRoutePlusOutput>()
                .ToListAsync();
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<(bool success, string message)> DeleteUserAsync(long userId, string email)
        {
            var model = await _userDbContext.TuserInfo.FirstOrDefaultAsync(f => f.FuserId == userId && f.Femail == email);
            if (model == null)
            {
                return (false, "用户不存在");
            }
            var roles = new UserRoleType[] { UserRoleType.None, UserRoleType.Buyer, UserRoleType.Seller, UserRoleType.Carrier };
            if (!roles.Contains(model.FuserRole.Value))
            {
                return (false, $"目前仅支持删除角色为：{string.Join("、", roles.Select(s => s.GetDescription()))}的用户");
            }
            DeleteUserResult result = _deleteUserRpcService.DeleteUser(email, userId, (int)model.FuserRole.Value);
            return (result.IsSuccess, result.Message);
        }
    }
}