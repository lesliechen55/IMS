using System;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using YQTrack.Core.Backend.Admin.CarrierTrack.JobService;
using YQTrack.Log;

namespace YQTrack.Core.Backend.Admin.Web
{
    public static class JobRunner
    {
        public static void InitJobRunner(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    // 解析周期任务管理器
                    var recurringJobManager = services.GetRequiredService<IRecurringJobManager>();
                    // 每天早上8:30点的时候执行
                    recurringJobManager.AddOrUpdate("CarrierTrack", () => JobService.GenerateUserReport(), "00 30 08 * * ?", TimeZoneInfo.Local);
                    logger.LogInformation($"{nameof(JobRunner)}添加任务成功");

                    LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Info, "InitJobRunner"), new
                    {
                        Msg = $"{nameof(JobRunner)}添加任务成功"
                    });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred JobRunner.");
                    LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Error, "InitJobRunner"), ex, new
                    {
                        Msg = "An error occurred JobRunner."
                    });
                }
            }
        }
    }
}