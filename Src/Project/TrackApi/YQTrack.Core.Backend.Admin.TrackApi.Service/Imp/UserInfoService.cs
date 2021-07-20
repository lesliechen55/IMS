using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.TrackApi.Data;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Input;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.TrackApi.Data.Models;
using YQTrack.Core.Backend.Enums.TrackApi;
using YQTrack.Service.Standard.User.Interface;
using YQTrack.Service.Standard.User.Models;
using YQTrack.Core.Backend.Admin.User.Service;
using AutoMapper.QueryableExtensions;
using System.Data.SqlClient;
using Dapper;
using YQTrack.Backend.Models;
using YQTrack.Backend.Models.Enums;
using YQTrack.Backend.Sharding;
using YQTrack.Backend.Enums;
using YQTrack.Utility;

namespace YQTrack.Core.Backend.Admin.TrackApi.Service.Imp
{
    public class UserInfoService : IUserInfoService
    {
        private readonly ApiUserDbContext _dbApiUserContext;
        private readonly IUserService _userService;
        private ApiTrackDbContext _dbApiTrackContext;
        private readonly IUserAutoRegister _userAutoRegister;
        public string ApiTrackString { get; set; }

        public UserInfoService(ApiUserDbContext dbApiUserContext, ApiTrackDbContext dbApiTrackContext, IUserService userService, IUserAutoRegister userAutoRegister)
        {
            _dbApiUserContext = dbApiUserContext;
            _dbApiTrackContext = dbApiTrackContext;
            _userService = userService;
            _userAutoRegister = userAutoRegister;
        }

        /// <summary>
        /// 更换分库
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task SetDBRouteAsync(long userId)
        {
            var userRoute = await _userService.GetUserDataRouteAsync(userId);
            if (userRoute == null)
            {
                return;
            }
            var dataRouteModel = new DataRouteModel()
            {
                NodeId = userRoute.FnodeId.Value,
                DbNo = userRoute.FdbNo.Value,
                TableNo = userRoute.FtableNo.Value,
                UserRole = (byte)userRoute.FuserRole.Value,
                IsArchived = false,
                IsWrite = true
            };
            var connectionString = DBShardingRouteFactory.GetDBConnStr(YQDbType.ApiTrack.ToString(), dataRouteModel);
            if (!string.IsNullOrWhiteSpace(connectionString) && _dbApiTrackContext.Database.GetDbConnection().ConnectionString != connectionString)
            {
                _dbApiTrackContext.Database.GetDbConnection().ConnectionString = connectionString;
            }
        }

        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        /// <param name="input">用户分页列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<UserInfoPageDataOutput> outputs, int total)> GetPageDataAsync(UserInfoPageDataInput input)
        {
            var apiUserInfos = _dbApiUserContext.TApiUserInfo
                .WhereIf(() => input.UserId.HasValue, x => x.FUserId == input.UserId.Value)
                .WhereIf(() => input.ApiState.HasValue, x => x.FApiState == input.ApiState.Value)
                .WhereIf(() => input.StartTime.HasValue, x => x.FCreatedTime >= input.StartTime.Value.ToUniversalTime())
                .WhereIf(() => input.EndTime.HasValue, x => x.FCreatedTime <= input.EndTime.Value.AddDays(1).ToUniversalTime())
                .WhereIf(() => !string.IsNullOrWhiteSpace(input.UserName), x => x.FUserName.Contains(input.UserName.Trim()));

            var queryable = from userInfo in apiUserInfos
                            join config in _dbApiUserContext.TApiUserConfig on userInfo.FUserId equals config.FUserId into bb
                            from config in bb.DefaultIfEmpty()
                            select new UserInfoPageDataOutput
                            {
                                FUserId = userInfo.FUserId,
                                FUserName = userInfo.FUserName,
                                FUserNo = userInfo.FUserNo,
                                //FMaxTrackReq = config == null ? 0 : config.FMaxTrackReq,
                                FContactPhone = userInfo.FContactPhone,
                                FContactName = userInfo.FContactName,
                                FApiState = userInfo.FApiState,
                                FCreatedTime = userInfo.FCreatedTime,
                                FScheduleFrequency = config == null ? (byte?)null : config.FScheduleFrequency,
                                FGiftQuota = config == null ? (byte?)null : config.FGiftQuota,
                                FEmail = userInfo.FEmail,
                                FContactEmail = userInfo.FContactEmail
                            };

            var count = await queryable.CountAsync();
            var outputs = await queryable
                .OrderByDescending(x => x.FCreatedTime)
                .ToPage(input.Page, input.Limit)
                .ToListAsync();
            //所有可用的数据库连接
            List<string> apiTrackConnectionString = DBShardingRouteFactory.GetDBConnectStrings(YQDbType.ApiTrack.ToString());
            List<QuoteOutput> quoteOutputList = new List<QuoteOutput>();
            IEnumerable<long> userIds = outputs.Select(s => s.FUserId);
            foreach (string connStr in apiTrackConnectionString)
            {
                if (!string.IsNullOrWhiteSpace(connStr) && _dbApiTrackContext.Database.GetDbConnection().ConnectionString != connStr)
                {
                    _dbApiTrackContext.Database.GetDbConnection().ConnectionString = connStr;
                    quoteOutputList.AddRange(await _dbApiTrackContext.TTrackQuota
                        .Where(w => userIds.Contains(w.FUserId))
                        .ProjectTo<QuoteOutput>()
                        .ToListAsync());
                }
            }
            outputs.ForEach(f =>
            {
                QuoteOutput quoteOutput = quoteOutputList.SingleOrDefault(s => s.FUserId == f.FUserId);
                if (quoteOutput == null)
                {
                    quoteOutput = new QuoteOutput();
                }
                else if (quoteOutput.FToday.Date != DateTime.UtcNow.Date)
                {
                    quoteOutput.FTodayUsed = 0;
                }
                f.FRemain = quoteOutput.FRemain;
                f.FTodayUsed = quoteOutput.FTodayUsed;
            });

            return (outputs, count);
        }
        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserInfoOutput> GetByIdAsync(int id)
        {
            var output = await (from userInfo in _dbApiUserContext.TApiUserInfo
                                join config in _dbApiUserContext.TApiUserConfig
                                on userInfo.FUserId equals config.FUserId
                                into bb
                                from config in bb.DefaultIfEmpty()
                                select new UserInfoOutput
                                {
                                    FUserId = userInfo.FUserId,
                                    FUserName = userInfo.FUserName,
                                    FUserNo = userInfo.FUserNo,
                                    FEmail = userInfo.FEmail,
                                    //FTrackFrequency = userInfo.FTrackFrequency,
                                    FCurrency = userInfo.FCurrency,
                                    //FMaxTrackReq = config == null ? 0 : config.FMaxTrackReq,
                                    FCompanyName = userInfo.FCompanyName,
                                    FVATNo = userInfo.FVATNo,
                                    FCountry = userInfo.FCountry,
                                    FAddress = userInfo.FAddress,
                                    FContactPhone = userInfo.FContactPhone,
                                    FContactName = userInfo.FContactName,
                                    FContactEmail = userInfo.FContactEmail,
                                    FIsChinese = userInfo.FIsChinese,
                                    FApiState = userInfo.FApiState,
                                    FRemark = userInfo.FRemark,
                                    FCreatedTime = userInfo.FCreatedTime,
                                    FScheduleFrequency = config == null ? (byte)ScheduleFrequency.High : config.FScheduleFrequency,
                                    FGiftQuota = config == null ? (byte)0 : config.FGiftQuota
                                }).Where(w => w.FUserNo == id)
                                .SingleOrDefaultAsync();

            return output;
        }

        /// <summary>
        /// 获取可用的最大的用户编号
        /// </summary>
        /// <returns></returns>
        public async Task<short> GetMaxUserNo()
        {
            const string strSql = @"select top 1 max(FUserNo)-1 as maxNo 
                              from TApiUserInfo
                              group by FUserNo having max(FUserNo) - 1 not in(select FUserNo from TApiUserInfo)
                              order by maxNo desc";
            short userNo;
            using (var connection = new SqlConnection(_dbApiUserContext.Database.GetDbConnection().ConnectionString))
            {
                userNo = await connection.ExecuteScalarAsync<short>(new CommandDefinition(strSql));
            }
            return userNo == 0 ? short.MaxValue : userNo;
        }

        /// <summary>
        /// 根据ID获取额度消耗实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserInfoViewOutput> GetViewDataByIdAsync(int id)
        {
            var output = await (from userInfo in _dbApiUserContext.TApiUserInfo
                                join config in _dbApiUserContext.TApiUserConfig
                                on userInfo.FUserId equals config.FUserId
                                into bb
                                from config in bb.DefaultIfEmpty()
                                select new UserInfoViewOutput
                                {
                                    FUserId = userInfo.FUserId,
                                    FUserName = userInfo.FUserName,
                                    FUserNo = userInfo.FUserNo,
                                    FMaxTrackReq = config == null ? 0 : config.FMaxTrackReq
                                })
                                .Where(w => w.FUserNo == id)
                                .SingleOrDefaultAsync();
            if (output?.FUserId == null) throw new BusinessException(nameof(id), id.ToString());
            await SetDBRouteAsync(output.FUserId.Value);
            var quota = _dbApiTrackContext.TTrackQuota.SingleOrDefault(s => s.FUserId == output.FUserId);
            if (quota == null)
            {
                quota = new TTrackQuota();
            }
            else if (quota.FToday.Date != DateTime.UtcNow.Date)
            {
                quota.FTodayUsed = 0;
            }
            output.FQuota = quota.FQuota;
            output.FUsed = quota.FUsed;
            output.FTodayUsed = quota.FTodayUsed;
            output.FRemain = quota.FRemain;
            return output;
        }

        /// <summary>
        /// 生成用户访问密钥
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <param name="secretSeed">密钥种子</param>
        /// <returns></returns>
        public static string GetSecretKey(int userNo, DateTime? secretSeed)
        {
            if (secretSeed.HasValue)
            {
                return SecurityExtend.MD5Encrypt($"{ userNo.ToString()}{secretSeed.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff")}");
            }
            else
            {
                return SecurityExtend.MD5Encrypt($"{ userNo.ToString()}");
            }
        }

        /// <summary>
        /// 根据ID获取用户配置实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserConfigOutput> GetUserConfigByIdAsync(int id)
        {
            var output = await (from userInfo in _dbApiUserContext.TApiUserInfo
                                join config in _dbApiUserContext.TApiUserConfig
                                on userInfo.FUserId equals config.FUserId
                                into bb
                                from config in bb.DefaultIfEmpty()
                                select new UserConfigOutput
                                {
                                    FUserId = userInfo.FUserId,
                                    FUserName = userInfo.FUserName,
                                    FUserNo = userInfo.FUserNo,
                                    FEmail = userInfo.FEmail,
                                    FContactEmail = userInfo.FContactEmail,
                                    FContactName = userInfo.FContactName,
                                    FContactPhone = userInfo.FContactPhone,
                                    FWebHook = config == null ? "" : config.FWebHook,
                                    FSecretSeed = (config == null || !config.FSecretSeed.HasValue) ? DateTime.UtcNow : config.FSecretSeed.Value,
                                    FScheduleFrequency = config == null ? ScheduleFrequency.None : (ScheduleFrequency)config.FScheduleFrequency,
                                    FIPWhiteList = config == null ? "" : config.FIPWhiteList,
                                    FApiNotify = config == null ? "" : config.FApiNotify
                                })
                                .Where(w => w.FUserNo == id)
                                .SingleOrDefaultAsync();
            return output;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<bool> AddAsync(UserInfoEditInput input, int operatorId)
        {
            if (await _dbApiUserContext.TApiUserInfo.AnyAsync(a => a.FUserId == 0))
            {
                throw new BusinessException("有一条未处理的用户数据，请处理后再添加");
            }
            if (await _dbApiUserContext.TApiUserInfo.AnyAsync(a => a.FUserNo == input.FUserNo))
            {
                return false;
            }
            if (await _dbApiUserContext.TApiUserInfo.AnyAsync(a => a.FEmail == input.FEmail))
            {
                throw new BusinessException($"注册API用户失败：用户邮箱已存在");
            }
            var userInfo = new TApiUserInfo()
            {
                FUserName = input.FUserName,
                FUserNo = input.FUserNo,
                FEmail = input.FEmail,
                //FTrackFrequency = input.FTrackFrequency,
                FApiState = (byte)ApiState.Available,
                FAuditState = (byte)AuditState.Accessed,
                FCompanyName = input.FCompanyName,
                FVATNo = input.FVATNo,
                FAddress = input.FAddress,
                FCountry = input.FCountry,
                FContactEmail = input.FContactEmail,
                FContactName = input.FContactName,
                FContactPhone = input.FContactPhone,
                FCurrency = input.FCurrency,
                FIsChinese = input.FIsChinese,
                FCreatedTime = DateTime.UtcNow,
                FCreatedBy = operatorId,
                FUpdateTime = DateTime.UtcNow,
                FUpdateBy = operatorId,
                FRemark = input.FRemark
            };
            await _dbApiUserContext.TApiUserInfo.AddAsync(userInfo);
            if (await _dbApiUserContext.SaveChangesAsync() <= 0)
            {
                throw new BusinessException($"注册API用户失败：写入用户数据失败");
            }
            var inModel = new ApiUserRegisterInModel()
            {
                Email = userInfo.FEmail,
                NickName = userInfo.FUserName,
                IsCN = userInfo.FIsChinese
            };
            ApiUserRegisterOutModel outModel = null;
            try
            {
                outModel = await _userAutoRegister.ApiUserRegister(inModel);
                if (!outModel.IsSuccess)
                {
                    if (outModel.Code == (int)RegisterErrorCode.EmailIsReg)
                    {
                        _dbApiUserContext.TApiUserInfo.Remove(userInfo);
                        await _dbApiUserContext.SaveChangesAsync();
                    }
                    throw new BusinessException($"注册API用户失败：{outModel.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException)
                {
                    throw new BusinessException("服务端接口无响应，请联系管理员");
                }
                else
                {
                    throw new BusinessException(ex.Message);
                }
            }
            userInfo.FUserId = outModel.UserId;
            return await ReqRegister(operatorId, userInfo, outModel, input.ScheduleFrequency, input.GiftQuota);
        }

        /// <summary>
        /// 注册重试
        /// </summary>
        /// <param name="userNo"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<bool> ReregisterAsync(int userNo, int operatorId)
        {
            var userInfo = await _dbApiUserContext.TApiUserInfo.SingleOrDefaultAsync(s => s.FUserNo == userNo);
            if (null == userInfo)
            {
                throw new BusinessException($"{nameof(userNo)}参数错误,该用户不存在");
            }
            if (userInfo.FUserId != 0)
            {
                throw new BusinessException($"该用户信息完整，无需重试");
            }
            var inModel = new ApiUserRegisterInModel()
            {
                Email = userInfo.FEmail,
                NickName = userInfo.FUserName,
                IsCN = userInfo.FIsChinese
            };
            ApiUserRegisterOutModel outModel = null;
            try
            {
                outModel = await _userAutoRegister.ApiUserRegisterRetry(inModel);
                if (!outModel.IsSuccess)
                {
                    if (outModel.Code == (int)RegisterErrorCode.EmailIsReg)
                    {
                        _dbApiUserContext.TApiUserInfo.Remove(userInfo);
                        await _dbApiUserContext.SaveChangesAsync();
                    }
                    throw new BusinessException($"重试失败：{outModel.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException)
                {
                    throw new BusinessException("服务端接口无响应，请联系管理员");
                }
                else
                {
                    throw new BusinessException(ex.Message);
                }
            }
            userInfo.FUserId = outModel.UserId;
            return await ReqRegister(operatorId, userInfo, outModel);
        }

        /// <summary>
        /// 请求注册接口，回写用户数据(重新注册情况下拿不到用户填写的配置参数,使用下面的默认值)
        /// </summary>
        /// <param name="operatorId"></param>
        /// <param name="userInfo"></param>
        /// <param name="outModel"></param>
        /// <param name="scheduleFrequency">调度频率 默认:高</param>
        /// <param name="giftQuota">下单赠送配额 默认:0</param>
        /// <returns></returns>
        private async Task<bool> ReqRegister(int operatorId, TApiUserInfo userInfo, ApiUserRegisterOutModel outModel, ScheduleFrequency scheduleFrequency = ScheduleFrequency.High, byte giftQuota = 0)
        {
            var config = new TApiUserConfig()
            {
                FUserId = userInfo.FUserId,
                //FMaxTrackReq = input.FMaxTrackReq,
                FSecretSeed = DateTime.UtcNow,
                FCreatedBy = operatorId,
                FCreatedTime = DateTime.UtcNow,
                FUpdateBy = operatorId,
                FUpdateTime = DateTime.UtcNow,
                FScheduleFrequency = (byte)scheduleFrequency,
                FGiftQuota = giftQuota
            };
            await _dbApiUserContext.TApiUserConfig.AddAsync(config);
            if (await _dbApiUserContext.SaveChangesAsync() <= 0)
            {
                throw new BusinessException($"注册API用户失败：写入用户配置数据失败");
            }

            var quotaModel = new TTrackQuota
            {
                FUserId = userInfo.FUserId,
                FQuota = 100,
                FToday = DateTime.UtcNow,
                FUsed = 0,
                FTodayUsed = 0
            };
            var dataRouteModel = new DataRouteModel()
            {
                NodeId = outModel.NodeId,
                DbNo = outModel.DbNo,
                TableNo = outModel.TableNo,
                UserRole = (byte)outModel.UserRole,
                IsArchived = false,
                IsWrite = true
            };
            var connectionString = DBShardingRouteFactory.GetDBConnStr(YQDbType.ApiTrack.ToString(), dataRouteModel);
            if (!string.IsNullOrWhiteSpace(connectionString) && _dbApiTrackContext.Database.GetDbConnection().ConnectionString != connectionString)
            {
                _dbApiTrackContext.Database.GetDbConnection().ConnectionString = connectionString;
            }
            _dbApiTrackContext.TTrackQuota.Add(quotaModel);
            return await _dbApiTrackContext.SaveChangesAsync() > 0;
        }


        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task<bool> EditAsync(UserInfoEditInput input, int operatorId)
        {
            var userInfo = await _dbApiUserContext.TApiUserInfo.SingleOrDefaultAsync(s => s.FUserNo == input.FUserNo);
            if (null == userInfo)
            {
                throw new BusinessException($"{nameof(input.FUserNo)}参数错误,该用户不存在");
            }
            userInfo.FUserName = input.FUserName;
            //userInfo.FEmail = input.FEmail;
            //userInfo.FTrackFrequency = input.FTrackFrequency;
            userInfo.FCompanyName = input.FCompanyName;
            userInfo.FVATNo = input.FVATNo;
            userInfo.FAddress = input.FAddress;
            userInfo.FCountry = input.FCountry;
            userInfo.FContactEmail = input.FContactEmail;
            userInfo.FContactName = input.FContactName;
            userInfo.FContactPhone = input.FContactPhone;
            userInfo.FCurrency = input.FCurrency;
            userInfo.FIsChinese = input.FIsChinese;
            userInfo.FUpdateTime = DateTime.UtcNow;
            userInfo.FUpdateBy = operatorId;
            userInfo.FRemark = input.FRemark;

            var config = await _dbApiUserContext.TApiUserConfig.SingleOrDefaultAsync(x => x.FUserId == userInfo.FUserId);
            if (config == null)
            {
                throw new BusinessException($"{nameof(input.FUserNo)}参数错误,该用户的配置不存在");
            }

            config.FScheduleFrequency = (byte)input.ScheduleFrequency;
            config.FGiftQuota = input.GiftQuota;

            return await _dbApiUserContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改API状态
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorId">操作人</param>
        /// <returns></returns>
        public async Task ChangeStatusAsync(ChangeApiStateInput input, int operatorId)
        {
            var userInfo = await _dbApiUserContext.TApiUserInfo.SingleOrDefaultAsync(s => s.FUserNo == input.FUserNo);
            //TApiUserInfo userInfo = _dbApiUserContext.TApiUserInfo.Where(w=>w.FUserNo==input.FUserNo).FirstOrDefault();
            if (null == userInfo)
            {
                throw new BusinessException($"{nameof(input.FUserNo)}参数错误,该用户不存在");
            }
            userInfo.FApiState = input.FApiState;
            userInfo.FUpdateBy = operatorId;
            userInfo.FUpdateTime = DateTime.UtcNow;
            await _dbApiUserContext.SaveChangesAsync();
        }
    }
}
