using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();
            // 初始化数据库默认数据
            webHost.InitDbDefaultData();
            SettingsHelper.InitConfig();
            // 初始化定时任务
            webHost.InitJobRunner();
            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                    logging.AddConsole();
                });
    }
}
