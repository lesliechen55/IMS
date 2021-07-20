using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Models;
using YQTrack.Backend.Models.Enums;
using YQTrack.Backend.Sharding;
using YQTrack.Core.Backend.Admin.CarrierTrack.Core;
using YQTrack.Core.Backend.Admin.CarrierTrack.Data;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Input;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Output;
using YQTrack.Core.Backend.Admin.CarrierTrack.Service.Imp.Dto;
using YQTrack.Core.Backend.Admin.CommonService;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.Service;
using YQTrack.Core.Backend.Enums.Pay;
namespace YQTrack.Core.Backend.Admin.CarrierTrack.Service.Imp
{
    public class StatisticsService : IStatisticsService
    {
        private readonly CarrierTrackDbContext _carrierTrackDbContext;
        private readonly IUserInfoService _userInfoService;
        private readonly IUserService _userService;

        public StatisticsService(CarrierTrackDbContext carrierTrackDbContext,
            IUserInfoService userInfoService, IUserService userService)
        {
            _carrierTrackDbContext = carrierTrackDbContext;
            _userInfoService = userInfoService;
            _userService = userService;
        }

        #region 私有方法

        /// <summary>
        /// 根据用户路由动态切换数据库上下文的数据库连接字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task SetDbRouteAsync(long userId)
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
            if (!string.IsNullOrWhiteSpace(connectionString) && _carrierTrackDbContext.Database.GetDbConnection().ConnectionString != connectionString)
            {
                _carrierTrackDbContext.Database.GetDbConnection().ConnectionString = connectionString;
            }
        }

        #endregion

        /// <summary>
        /// 获取指定时间范围内前10个最高导入量的用户ID集合( 多库搜索汇总结果 )
        /// </summary>
        /// <returns></returns>
        private static async Task<IEnumerable<long>> GetImportTop10UserIdAsync(DateTime startDateTime, DateTime enDateTime, bool? enable)
        {
            var dbTypeRoute = DBShardingRouteFactory.GetDBTypeRule(YQDbType.CarrierTrack.ToString());
            if (dbTypeRoute == null || !dbTypeRoute.NodeRoutes.Any())
            {
                throw new BusinessException($"{YQDbType.CarrierTrack.ToString()}当前数据库类型找不到相关分库配置");
            }
            var routeList = new List<DataRouteModel>();
            foreach (var (key, value) in dbTypeRoute.NodeRoutes)
            {
                routeList.AddRange(value.DBRules.Select(dbInfo => new DataRouteModel
                {
                    NodeId = (byte)key,
                    DbNo = (byte)dbInfo.Key,
                    TableNo = 0,
                    UserRole = (byte)UserRoleType.Carrier,
                    IsArchived = false,
                    IsWrite = true
                }));
            }
            var connections = routeList.Select(s => DBShardingRouteFactory.GetDBConnStr((YQDbType.CarrierTrack.ToString()), s));
            var list = new List<TopTenQueryDto>();
            var enableSql = enable.HasValue ? $"and b.FEnable={(enable.Value ? 1 : 0)}" : string.Empty;
            foreach (var connectionStr in connections)
            {
                using (var connection = new SqlConnection(connectionStr))
                {
                    var cmd = $@"select top 10
	                                a.FUserId as UserId,
                                    isnull(sum(a.FSuccessInsertTotal),0) as SuccessInsertTotal
                                from dbo.TTrackUploadRecord as a
                                join dbo.TControl as b on a.FUserId = b.FUserId
                                where a.FCreateTime >= @startTime and a.FCreateTime < @endTime {enableSql}
                                group by a.FUserId
                                order by isnull(sum(a.FSuccessInsertTotal),0) desc
                                ";
                    var temp = (await connection.QueryAsync<TopTenQueryDto>(new CommandDefinition(cmd, new
                    {
                        startTime = startDateTime,
                        endTime = enDateTime
                    }))).ToList();
                    list.AddRange(temp);
                }
            }
            var userIdList = list.OrderByDescending(x => x.SuccessInsertTotal).Take(10).Select(x => x.UserId);
            return userIdList;
        }

        /// <summary>
        /// 获取指定时间段单个用户导入元数据
        /// </summary>
        /// <param name="chartDateType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ImportDbQueryDto>> GetImportDbQueryDataAsync(ChartDateType chartDateType,
            DateTime startTime, DateTime endTime, long userId)
        {
            List<ImportDbQueryDto> dtos;

            using (var connection =
                new SqlConnection(_carrierTrackDbContext.Database.GetDbConnection().ConnectionString))
            {
                string groupDateKeySql;
                switch (chartDateType)
                {
                    case ChartDateType.Day:
                        groupDateKeySql = @"convert(char(10), a.FCreateTime, 120)";
                        break;
                    case ChartDateType.Week:
                        groupDateKeySql = @"datepart(year,a.FCreateTime),datepart(WEEK,a.FCreateTime)";
                        break;
                    case ChartDateType.Month:
                        groupDateKeySql = @"convert(char(7),a.FCreateTime,120)";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(chartDateType), chartDateType, null);
                }

                var userIdSql = $@"and a.FUserId={userId}";

                var cmd = $@"
                    select a.FUserId as UserId,
                    {(chartDateType == ChartDateType.Week ? "CONCAT(datepart(year,a.FCreateTime),'第',datepart(WEEK,a.FCreateTime),'周')" : groupDateKeySql)} as DateFormat,
                    isnull(sum(a.FSuccessInsertTotal),0) as SuccessInsertTotal
                    from dbo.TTrackUploadRecord as a
                    where a.FCreateTime >= @startTime and a.FCreateTime < @endTime {userIdSql}
                    group by a.FUserId,{groupDateKeySql}
                    order by a.FUserId,{groupDateKeySql} asc
                ";
                dtos = (await connection.QueryAsync<ImportDbQueryDto>(new CommandDefinition(cmd, new
                {
                    startTime,
                    endTime
                }))).ToList();
            }
            return dtos;
        }

        /// <summary>
        /// 获取指定用户汇总多个数据库的数据返回
        /// </summary>
        /// <param name="chartDateType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="top10UserIdList"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ImportDbQueryDto>> GetImportDbQueryDataFromMultipleDbAsync(ChartDateType chartDateType, DateTime startTime, DateTime endTime, long[] top10UserIdList)
        {
            var dtos = new List<ImportDbQueryDto>();

            if (top10UserIdList == null || !top10UserIdList.Any()) return dtos;

            var routeOutputs = await _userService.GetUserDataRouteListAsync(top10UserIdList);

            if (!routeOutputs.Any())
            {
                throw new BusinessException($"用户找不到任何对应的数据路由信息错误!");
            }

            foreach (var userDataRouteOutput in routeOutputs)
            {
                if (userDataRouteOutput.FdbNo == 0)
                {
                    throw new BusinessException($"当前用户:{userDataRouteOutput.FuserId}路由信息错误,详情:{nameof(userDataRouteOutput.FdbNo)}:{userDataRouteOutput.FdbNo}");
                }
            }

            var connections = routeOutputs.Select(userRoute => DBShardingRouteFactory.GetDBConnStr(YQDbType.CarrierTrack.ToString(), new DataRouteModel
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
            }));

            var conStrList = connections.Distinct();

            foreach (var connectionStr in conStrList)
            {
                using (var connection =
                    new SqlConnection(connectionStr))
                {
                    string groupDateKeySql;
                    switch (chartDateType)
                    {
                        case ChartDateType.Day:
                            groupDateKeySql = @"convert(char(10), a.FCreateTime, 120)";
                            break;
                        case ChartDateType.Week:
                            groupDateKeySql = @"datepart(year,a.FCreateTime),datepart(WEEK,a.FCreateTime)";
                            break;
                        case ChartDateType.Month:
                            groupDateKeySql = @"convert(char(7),a.FCreateTime,120)";
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(chartDateType), chartDateType, null);
                    }

                    var top10Sql = $@"{ (top10UserIdList != null && top10UserIdList.Any() ? "and a.FUserId in @top10UserIdList" : "")}";

                    var cmd = $@"
                    select a.FUserId as UserId,
                    {(chartDateType == ChartDateType.Week ? "CONCAT(datepart(year,a.FCreateTime),'第',datepart(WEEK,a.FCreateTime),'周')" : groupDateKeySql)} as DateFormat,
                    isnull(sum(a.FSuccessInsertTotal),0) as SuccessInsertTotal
                    from dbo.TTrackUploadRecord as a
                    where a.FCreateTime >= @startTime and a.FCreateTime < @endTime {top10Sql}
                    group by a.FUserId,{groupDateKeySql}
                    order by a.FUserId,{groupDateKeySql} asc
                ";
                    var temp = (await connection.QueryAsync<ImportDbQueryDto>(new CommandDefinition(cmd, new
                    {
                        startTime,
                        endTime,
                        top10UserIdList
                    }))).ToList();
                    dtos.AddRange(temp);
                }
            }

            // 合并数据排序输出
            var result = dtos.OrderBy(x => x.UserId).ThenBy(x => x.DateFormat);

            return result;
        }

        /// <summary>
        /// 处理加功统计图表结果
        /// </summary>
        /// <param name="chartDateType"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="output"></param>
        /// <param name="dtos"></param>
        private static void ProcessChartOutput(ChartDateType chartDateType, DateTime startTime,
            DateTime endTime, ChartOutput output, IReadOnlyCollection<ImportDbQueryDto> dtos)
        {
            switch (chartDateType)
            {
                case ChartDateType.Day:
                    for (var date = startTime; date.Date < endTime; date = date.AddDays(1))
                    {
                        var dateStr = date.ToString("yyyy-MM-dd");
                        LoopProcessChartOutput(dateStr, output, dtos);
                    }
                    break;
                case ChartDateType.Week:
                    var weekFormatList = ChartDateTimeHelper.GetWeekFormatList(startTime, endTime);
                    foreach (var week in weekFormatList)
                    {
                        LoopProcessChartOutput(week, output, dtos);
                    }
                    break;
                case ChartDateType.Month:
                    for (var date = startTime; date.Date < endTime; date = date.AddMonths(1))
                    {
                        var dateStr = date.ToString("yyyy-MM");
                        LoopProcessChartOutput(dateStr, output, dtos);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(chartDateType), chartDateType, null);
            }
        }

        /// <summary>
        /// 循环处理每次日期节点数据
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="output"></param>
        /// <param name="dtos"></param>
        private static void LoopProcessChartOutput(string dateStr, ChartOutput output, IReadOnlyCollection<ImportDbQueryDto> dtos)
        {
            output.XAxisData.Add(dateStr);

            // 判断是否当前日期有数据存在
            if (dtos.Any(x => x.DateFormat == dateStr))
            {
                // 赋值给对应用户统计数据
                output.Series.ForEach(x =>
                {
                    var queryDto = dtos.FirstOrDefault(c => c.Email == x.Name && c.DateFormat == dateStr);
                    x.Data.Add(queryDto?.SuccessInsertTotal ?? 0);
                });
            }
            else
            {
                // 初始化所有情况为0
                output.Series.ForEach(x => x.Data.Add(0));
            }
        }

        public async Task<ChartOutput> GetTrackAnalysisOutputAsync(ChartDateType chartDateType, string email, bool? enable)
        {
            // 初始化搜索时间范围
            var (startTime, endTime) = ChartDateTimeHelper.InitTime(chartDateType);

            // 检查是否指定用户搜索
            var userId = await _userInfoService.GetUserIdByEmailAsync(email);

            // 构造返回结果
            var chartOutput = new ChartOutput
            {
                Title = "导入数统计"
            };

            // 根据条件查询数据库元数据
            IEnumerable<ImportDbQueryDto> importDbQueryData;
            var dictionary = new Dictionary<long, string>();
            if (userId.HasValue)
            {
                await SetDbRouteAsync(userId.Value);
                // 检查当前用户是否符合enable状态查询
                var userInfo = await _carrierTrackDbContext.TControl.SingleOrDefaultAsync(x => x.FUserId == userId.Value);
                if (userInfo == null)
                {
                    throw new BusinessException($"用户{userId.Value}在配置管理中找不到数据");
                }

                if (enable.HasValue)
                {
                    if (enable.Value && userInfo.FEnable == false)
                        throw new BusinessException($"用户{userId.Value}在配置管理中是禁用状态且查询条件是启用状态产生冲突");
                    if (enable.Value == false && userInfo.FEnable)
                        throw new BusinessException($"用户{userId.Value}在配置管理中是启用状态且查询条件是禁用状态产生冲突");
                }

                // 单个用户模式(根据用户路由信息找到指定存储库查询)
                chartOutput.Series = new List<SerieItemOutput>
                {
                    new SerieItemOutput
                    {
                        Name = email.Trim(),
                        Data = new List<decimal>()
                    }
                };
                dictionary.Add(userId.Value, email.Trim());
                // 根据用户切换数据库上下文的链接字符串
                await SetDbRouteAsync(userId.Value);
                importDbQueryData = await GetImportDbQueryDataAsync(chartDateType, startTime, endTime, userId.Value);
            }
            else
            {
                // 最多10个最高导入量的用户模式 , 注意 0 个用户的情况( 多库搜索汇总结果 )
                var top10UserId = (await GetImportTop10UserIdAsync(startTime, endTime, enable)).ToList();

                dictionary = await _userInfoService.GetEmailListByUserIdListAsync(top10UserId.ToArray());

                var serieItemOutputs = dictionary.Select(x => new SerieItemOutput
                {
                    Data = new List<decimal>(),
                    Name = x.Value
                }).ToList();

                chartOutput.Series = serieItemOutputs;

                importDbQueryData = await GetImportDbQueryDataFromMultipleDbAsync(chartDateType, startTime, endTime, top10UserId.ToArray());
            }

            // 匹配UserId与Email的关系
            var list = importDbQueryData.ToList();
            list.ForEach(x => { x.Email = dictionary[x.UserId].Trim(); });

            // 处理数据库元数据
            ProcessChartOutput(chartDateType, startTime, endTime, chartOutput, list);

            return chartOutput;
        }

        public async Task<(IEnumerable<UserMarkLogPageDataOutput> outputs, int total)> GetUserMarkLogPageDataAsync(UserMarkLogPageDataInput input)
        {
            if (input.Email.IsNullOrWhiteSpace())
            {
                throw new BusinessException($"{nameof(input.Email)}参数为空错误");
            }

            var userId = await _userInfoService.GetUserIdByEmailAsync(input.Email.Trim());
            if (!userId.HasValue)
            {
                throw new BusinessException($"{nameof(input.Email)}值错误,找不到对应用户");
            }

            await SetDbRouteAsync(userId.Value);

            var query = _carrierTrackDbContext.TUserMarkLog.Where(x => x.FUserId == userId.Value);
            var count = await query.CountAsync();
            var outputs = await query.OrderByDescending(x => x.FCreateTime).ToPage(input.Page, input.Limit).ProjectTo<UserMarkLogPageDataOutput>().ToListAsync();

            var userEmailList = await _userInfoService.GetEmailListByUserIdListAsync(outputs.Select(x => x.FUserId).ToArray());

            outputs.ForEach(x =>
            {
                x.FEmail = userEmailList[x.FUserId]?.Trim();
            });

            return (outputs, count);
        }

        public async Task<IEnumerable<ReportOutput>> GetExportAsync(DateTime startDate, DateTime endDate)
        {
            var list = new List<ReportOutput>();
            for (var date = startDate; date.Date <= endDate; date = date.AddDays(1))
            {
                list.Add(new ReportOutput { Date = date, UserImportOutputs = new List<UserImportOutput>() });
            }
            var allDbConnections = DbConnectionTool.GetAllDbConnections();
            foreach (var dbConnection in allDbConnections)
            {
                if (!_carrierTrackDbContext.Database.GetDbConnection().ConnectionString.Equals(dbConnection, StringComparison.InvariantCulture))
                {
                    _carrierTrackDbContext.Database.GetDbConnection().ConnectionString = dbConnection;
                }
                var reports = await _carrierTrackDbContext.TReport.Where(x => x.FDate >= startDate && x.FDate <= endDate).OrderBy(x => x.FDate).AsNoTracking().ToListAsync();
                if (!reports.Any()) continue;
                foreach (var item in list)
                {
                    item.UserImportOutputs.AddRange(
                        reports.Where(x => x.FDate == item.Date).Select(x => new UserImportOutput
                        {
                            Email = x.FEmail,
                            Import = x.FImport
                        }));
                }
            }
            return list;
        }

        public async Task<IEnumerable<ExportUserMarkLogOutput>> GetExportUserMarkLogAsync(string email, DateTime? startDate, DateTime? endDate)
        {
            var userId = await _userInfoService.GetUserIdByEmailAsync(email);
            if (userId.HasValue)
            {
                await SetDbRouteAsync(userId.Value);
                var outputs = await _carrierTrackDbContext.TUserMarkLog
                    .Where(x => x.FUserId == userId.Value)
                    .WhereIf(() => startDate.HasValue, x => x.FCreateTime >= startDate.Value)
                    .WhereIf(() => endDate.HasValue, x => x.FCreateTime < endDate.Value.AddDays(1))
                    .OrderByDescending(x => x.FCreateTime).ProjectTo<ExportUserMarkLogOutput>().ToListAsync();
                outputs.ForEach(x => x.FEmail = email);
                return outputs;
            }

            var outputList = new List<ExportUserMarkLogOutput>();
            var allDbConnections = DbConnectionTool.GetAllDbConnections();
            foreach (var dbConnection in allDbConnections)
            {
                if (_carrierTrackDbContext.Database.GetDbConnection().ConnectionString != dbConnection)
                {
                    _carrierTrackDbContext.Database.GetDbConnection().ConnectionString = dbConnection;
                }
                var outputs = await _carrierTrackDbContext.TUserMarkLog
                    .WhereIf(() => startDate.HasValue, x => x.FCreateTime >= startDate.Value)
                    .WhereIf(() => endDate.HasValue, x => x.FCreateTime < endDate.Value.AddDays(1))
                    .OrderByDescending(x => x.FCreateTime).ProjectTo<ExportUserMarkLogOutput>().ToListAsync();
                if (outputs.Any())
                    outputList.AddRange(outputs);
            }
            var userArr = outputList.Select(x => x.FUserId).Distinct().ToArray();
            var emailList = await _userInfoService.GetEmailListByUserIdListAsync(userArr);
            outputList.ForEach(x => x.FEmail = emailList.First(c => c.Key == x.FUserId).Value);
            outputList = outputList.OrderBy(x => x.FEmail).ThenByDescending(x => x.FCreateTime).ToList();
            return outputList;
        }
    }
}