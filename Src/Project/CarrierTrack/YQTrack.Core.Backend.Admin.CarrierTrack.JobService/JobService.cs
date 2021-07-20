using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using Dapper;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YQTrack.Core.Backend.Admin.CarrierTrack.Core;
using YQTrack.Core.Backend.Admin.CarrierTrack.Data;
using YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models;
using YQTrack.Core.Backend.Admin.CarrierTrack.JobService.Dto;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Log;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.JobService
{
    public class JobService
    {
        [DisableConcurrentExecution(60)]
        public static void GenerateUserReport()
        {
            var serviceProvider = GlobalServiceProvider.Current;
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var provider = serviceScope.ServiceProvider;
                var logger = provider.GetRequiredService<ILogger<JobService>>();

                var stopwatch = new Stopwatch();
                stopwatch.Start();
                logger.LogInformation($"{nameof(JobService)}.{nameof(GenerateUserReport)} {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} 开始执行任务...");

                LogHelper.LogObj(new LogDefinition(Log.LogLevel.Info, "GenerateUserReport"), new
                {
                    Msg = $"{nameof(JobService)}.{nameof(GenerateUserReport)} {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} 开始执行任务..."
                });

                var dbConnections = DbConnectionTool.GetAllDbConnections();

                foreach (var dbConnection in dbConnections)
                {
                    var dbContext = provider.GetRequiredService<CarrierTrackDbContext>();
                    if (!dbContext.Database.GetDbConnection().ConnectionString.Equals(dbConnection, StringComparison.InvariantCulture))
                    {
                        dbContext.Database.GetDbConnection().ConnectionString = dbConnection;
                    }
                    var userList = dbContext.TControl.Where(x => x.FEnable).Select(x => new { x.FUserId, x.FEmail }).OrderBy(x => x.FEmail).ToList();
                    if (!userList.Any())
                    {
                        logger.LogWarning($"数据库:{dbContext.Database.GetDbConnection().Database},找不到任何启用的货代用户同于数据统计报告");

                        LogHelper.LogObj(new LogDefinition(Log.LogLevel.Warn, "GenerateUserReport"), new
                        {
                            Msg = $"数据库:{dbContext.Database.GetDbConnection().Database},找不到任何启用的货代用户同于数据统计报告"
                        });

                        continue;
                    }

                    List<ImportDbQueryDto> list;
                    var userIdArr = userList.Select(x => x.FUserId).ToArray();
                    const string groupDateKeySql = @"convert(char(10), a.FCreateTime, 120)";
                    var utcNowDate = DateTime.UtcNow.Date;
                    var cmd = $@"
                        select a.FUserId as UserId,
                        {groupDateKeySql} as DateFormat,
                        isnull(sum(a.FSuccessInsertTotal),0) as SuccessInsertTotal
                        from dbo.TTrackUploadRecord as a
                        where a.FCreateTime >= @startTime and a.FCreateTime < @endTime and a.FUserId in @userIdList
                        group by a.FUserId,{groupDateKeySql}
                        order by a.FUserId,{groupDateKeySql} asc
                    ";
                    using (var connection = new SqlConnection(dbConnection))
                    {
                        list = connection.Query<ImportDbQueryDto>(new CommandDefinition(cmd, new
                        {
                            startTime = utcNowDate.AddDays(-1),
                            endTime = utcNowDate,
                            userIdList = userIdArr
                        })).ToList();
                    }
                    if (!list.Any())
                    {
                        logger.LogWarning($"数据库:{dbContext.Database.GetDbConnection().Database},单号导入表找不到任何对应用户的导入记录");

                        LogHelper.LogObj(new LogDefinition(Log.LogLevel.Warn, "GenerateUserReport"), new
                        {
                            Msg = $"数据库:{dbContext.Database.GetDbConnection().Database},单号导入表找不到任何对应用户的导入记录"
                        });

                        continue;
                    }

                    list.ForEach(x => x.Email = userList.First(c => c.FUserId == x.UserId).FEmail);

                    var insert = 0;
                    var update = 0;
                    foreach (var dto in list)
                    {
                        var dateTime = DateTime.Parse(dto.DateFormat);
                        var report = dbContext.TReport.SingleOrDefault(x => x.FUserId == dto.UserId && x.FDate == dateTime);
                        if (report == null)
                        {
                            dbContext.TReport.Add(new TReport
                            {
                                FId = IdHelper.GetGenerateId(),
                                FCreateBy = 1,
                                FDate = dateTime,
                                FEmail = dto.Email,
                                FImport = dto.SuccessInsertTotal,
                                FUserId = dto.UserId
                            });
                            insert++;
                        }
                        else
                        {
                            report.FImport = dto.SuccessInsertTotal;
                            report.FUpdateBy = 2;
                            report.FUpdateTime = DateTime.UtcNow;
                            update++;
                        }
                    }

                    dbContext.SaveChanges();

                    logger.LogInformation($"数据库:{dbContext.Database.GetDbConnection().Database},成功插入:{insert}条统计记录,成功更新:{update}条统计记录");

                    LogHelper.LogObj(new LogDefinition(Log.LogLevel.Info, "GenerateUserReport"), new
                    {
                        Msg = $"数据库:{dbContext.Database.GetDbConnection().Database},成功插入:{insert}条统计记录,成功更新:{update}条统计记录"
                    });
                }

                stopwatch.Stop();
                logger.LogInformation($"{nameof(JobService)}.{nameof(GenerateUserReport)} {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} 执行任务完成...耗时:{stopwatch.ElapsedMilliseconds} ms");

                LogHelper.LogObj(new LogDefinition(Log.LogLevel.Info, "GenerateUserReport"), new
                {
                    Msg = $"{nameof(JobService)}.{nameof(GenerateUserReport)} {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} 执行任务完成...耗时:{stopwatch.ElapsedMilliseconds} ms"
                });
            }
        }
    }
}
