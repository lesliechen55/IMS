using AspectCore.Extensions.DependencyInjection;
using AspectCore.Injector;
using AutoMapper;
using FluentValidation.AspNetCore;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using YQTrack.Core.Backend.Admin.CarrierTrack.Data;
using YQTrack.Core.Backend.Admin.CommonService;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Data;
using YQTrack.Core.Backend.Admin.Data.MediatR;
using YQTrack.Core.Backend.Admin.Deals.Data;
using YQTrack.Core.Backend.Admin.Freight.Data;
using YQTrack.Core.Backend.Admin.Freight.Service.RemoteApi;
using YQTrack.Core.Backend.Admin.Log.Data;
using YQTrack.Core.Backend.Admin.Message.Data;
using YQTrack.Core.Backend.Admin.Message.Service.RemoteApi;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.TrackApi.Data;
using YQTrack.Core.Backend.Admin.TrackApi.DTO;
using YQTrack.Core.Backend.Admin.User.Data;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.Web.Models.Request.Validators;
using YQTrack.Core.Backend.Admin.WebCore;
using YQTrack.SRVI.Payment.Interface;
using YQTrack.Configuration;
using YQTrack.RabbitMQ;
using ApiResult = YQTrack.Core.Backend.Admin.WebCore.ApiResult;
using YQTrack.SRVI.DeleteUser;
using YQTrack.Core.Backend.Admin.Seller.Data;
using YQTrack.Service.Standard.User.Interface;
using YQTrack.Core.Backend.Admin.DTO.Output;
using YQTrack.Backend.Email.Repository;

namespace YQTrack.Core.Backend.Admin.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // 设置cookie相关配置
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // 反射当前项目所引用程序集
            var allTypes = Assembly
                .GetEntryAssembly()
                .GetReferencedAssemblies().Where(x => x.FullName.StartsWith("YQTrack"))
                .Select(Assembly.Load)
                .SelectMany(x => x.ExportedTypes)
                .Concat(Assembly.GetExecutingAssembly().ExportedTypes)
                .Concat(typeof(IUserInfoService).Assembly.ExportedTypes)
                .ToArray();

            // 批量注册automapper映射配置文件
            var profileTypes = allTypes.Where(x => x.IsSubclassOf(typeof(Profile)) && !x.IsAbstract);
            Mapper.Initialize(x => x.AddProfiles(profileTypes));
            Mapper.Configuration.CompileMappings();
            var mapper = Mapper.Configuration.CreateMapper();
            services.AddSingleton(mapper);

            // 注册cookie的身份认证
            services.AddAuthentication(x =>
                    {
                        x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        x.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                .AddCookie(x =>
                {
                    x.LoginPath = "/home/login";
                    x.AccessDeniedPath = "/home/forbid";
                    x.LogoutPath = "/home/logout";
                    x.Cookie.SameSite = SameSiteMode.None;
                    x.Cookie.SecurePolicy = CookieSecurePolicy.None;
                });

            // 添加 Bearer 验证方式用于API验证
            services.AddAuthentication(x =>
                {
                    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        ValidIssuer = AppConfig.Issuer,
                        ValidAudience = AppConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppConfig.IssuerSigningKey))

                        /***********************************TokenValidationParameters的参数默认值***********************************/
                        // RequireSignedTokens = true,
                        // SaveSigninToken = false,
                        // ValidateActor = false,
                        // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                        // ValidateAudience = true,
                        // ValidateIssuer = true, 
                        // ValidateIssuerSigningKey = false,
                        // 是否要求Token的Claims中必须包含Expires
                        // RequireExpirationTime = true,
                        // 允许的服务器时间偏移量
                        // ClockSkew = TimeSpan.FromSeconds(300),
                        // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        // ValidateLifetime = true
                    };
                });

            // 注册mvc管道以及配置设置项
            services.AddMvc(x =>
                {
                    x.Filters.Add<GlobalExceptionFilter>(); // 添加全局异常过滤器
                    x.AllowValidatingTopLevelNodes = true; // 显示启用顶级节点验证
                })
                .AddControllersAsServices()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(
                    x =>
                    {
                        x.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                        x.SerializerSettings.Formatting = Formatting.Indented;
                    });

            // 配置api相关的基础属性
            services.Configure<ApiBehaviorOptions>(x =>
            {
                x.SuppressModelStateInvalidFilter = true;
            });

            // 注册httpcontext上下文
            services.AddHttpContextAccessor();

            // 注册内存级缓存
            services.AddMemoryCache();

            // 添加一个分布式缓存
            services.AddDistributedMemoryCache();

            // 注册session
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(3);
                options.Cookie.HttpOnly = true;
            });

            #region 注册hangfire定时服务

            // Add Hangfire services.
            var hangfireConnection = Configuration.GetConnectionString("HangfireConnection");
            if (hangfireConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(hangfireConnection));
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(hangfireConnection, new SqlServerStorageOptions
                {
                    // 批量提交最大超时时间
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    // 避免运行长时间的事务和数据库链接
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    // 任务队列轮询间隔时间 Zero 会导致数据库长连接和DbLock
                    QueuePollInterval = TimeSpan.FromSeconds(30),
                    // 使用推荐的事务隔离级别
                    UseRecommendedIsolationLevel = true,
                    // 出列的时候使用页锁
                    UsePageLocksOnDequeue = true,
                    // 禁用全局锁
                    DisableGlobalLocks = true,
                    // Add UseFineGrainedLocks option to avoid deadlocks in some theoretical cases.(避免数据库死锁)
                    UseFineGrainedLocks = true
                }));

            // Add the processing server as IHostedService
            services.AddHangfireServer();

            #endregion

            #region 注册数据库上下文
            // FreightDbContext 数据库上下文
            var freightConnection = Configuration.GetConnectionString("FreightDbContext");
            if (freightConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(freightConnection));
            services.AddDbContext<CarrierContext>(options => options.UseSqlServer(freightConnection));

            // AdminDbContext 数据库上下文
            var adminConnection = Configuration.GetConnectionString("AdminDbContext");
            if (adminConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(adminConnection));
            services.AddDbContext<AdminDbContext>(options => options.UseSqlServer(adminConnection));

            // UserDbContext 数据库上下文
            var userConnection = Configuration.GetConnectionString("UserDbContext");
            if (userConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(userConnection));
            services.AddDbContext<UserDbContext>(options => options.UseSqlServer(userConnection));

            // MessageDbContext 数据库上下文
            var msgConnection = Configuration.GetConnectionString("MessageDbContext");
            if (msgConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(msgConnection));
            services.AddDbContext<MessageDbContext>(options => options.UseSqlServer(msgConnection));

            // PayDbContext 数据库上下文
            var payConnection = Configuration.GetConnectionString("PayDbContext");
            if (payConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(payConnection));
            services.AddDbContext<PayDbContext>(options => options.UseSqlServer(payConnection));

            // DealsDbContext 数据库上下文
            var dealsConnection = Configuration.GetConnectionString("DealsDbContext");
            if (dealsConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(dealsConnection));
            services.AddDbContext<DealsDbContext>(options => options.UseSqlServer(dealsConnection));

            // LogDbContext 数据库上下文
            var logConnection = Configuration.GetConnectionString("LogDbContext");
            if (logConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(logConnection));
            services.AddDbContext<LogDbContext>(options => options.UseSqlServer(logConnection));

            // ApiUserDbContext 数据库上下文
            var apiUserConnection = Configuration.GetConnectionString("ApiUserDbContext");
            if (apiUserConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(apiUserConnection));
            services.AddDbContext<ApiUserDbContext>(options => options.UseSqlServer(apiUserConnection));

            // ApiTrackDbContext 数据库上下文
            var apiTrackConnection = Configuration.GetConnectionString("ApiTrackDbContext");
            if (apiTrackConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(apiTrackConnection));
            services.AddDbContext<ApiTrackDbContext>(options => options.UseSqlServer(apiTrackConnection));

            // CarrierTrackDbContext 数据库上下文
            var carrierTrackDbContext = Configuration.GetConnectionString("CarrierTrackDbContext");
            if (carrierTrackDbContext.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(carrierTrackDbContext));
            services.AddDbContext<CarrierTrackDbContext>(options => options.UseSqlServer(carrierTrackDbContext));

            // SellerOrderDBContext 数据库上下文
            var sellerOrderConnection = Configuration.GetConnectionString("SellerOrderDBContext");
            if (sellerOrderConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(sellerOrderConnection));
            services.AddDbContext<SellerOrderDBContext>(options => options.UseSqlServer(sellerOrderConnection));

            // SellerMessageDBContext 数据库上下文
            var sellerMessageConnection = Configuration.GetConnectionString("SellerMessageDBContext");
            if (sellerMessageConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(sellerMessageConnection));
            services.AddDbContext<SellerMessageDBContext>(options => options.UseSqlServer(sellerMessageConnection));

            // EmailDBContext 数据库上下文
            var emailConnection = Configuration.GetConnectionString("EmailDBContext");
            if (emailConnection.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(emailConnection));
            services.AddDbContext<EmailDbContext>(x => x.UseSqlServer(emailConnection));

            //by austin
            var realTimeDbContext = Configuration.GetConnectionString("SellerOrderDBContext");
            if (realTimeDbContext.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(realTimeDbContext));
            services.AddDbContext<RealTime.Data.RealTimeDbContext>(x => x.UseSqlServer(realTimeDbContext));

            #endregion

            #region 批量注册请求作用域的服务

            var serviceTypes = allTypes.Where(x => !x.IsAbstract && x.IsClass).Where(x => typeof(IScopeService).IsAssignableFrom(x));
            // 循环注册范围作用域服务
            foreach (var serviceType in serviceTypes)
            {
                var defaultInterface = serviceType.GetInterfaces().FirstOrDefault(x => x.Name.Contains(serviceType.Name));
                if (defaultInterface != null)
                {
                    services.AddScoped(defaultInterface, serviceType);
                }
                else
                {
                    services.AddScoped(serviceType);
                }
            }

            #endregion

            #region 批量注册请求单例的服务

            var singletonTypes = allTypes.Where(x => !x.IsAbstract && x.IsClass).Where(x => typeof(ISingletonService).IsAssignableFrom(x));
            // 循环注册单例服务
            foreach (var serviceType in singletonTypes)
            {
                var defaultInterface = serviceType.GetInterfaces().FirstOrDefault(x => x.Name.Contains(serviceType.Name));
                if (defaultInterface != null)
                {
                    services.AddSingleton(defaultInterface, serviceType);
                }
                else
                {
                    services.AddSingleton(serviceType);
                }
            }

            #endregion

            #region 批量注册请求瞬态的服务

            var transientTypes = allTypes.Where(x => !x.IsAbstract && x.IsClass).Where(x => typeof(ITransientService).IsAssignableFrom(x));
            // 循环注册瞬态服务
            foreach (var serviceType in transientTypes)
            {
                var defaultInterface = serviceType.GetInterfaces().FirstOrDefault(x => x.Name.Contains(serviceType.Name));
                if (defaultInterface != null)
                {
                    services.AddTransient(defaultInterface, serviceType);
                }
                else
                {
                    services.AddTransient(serviceType);
                }
            }

            #endregion


            // 注册 HttpClient
            services.AddHttpClient();

            #region 注册远程调用地址配置类

            services.Configure<FreightConfig>(Configuration.GetSection("Freight") ?? throw new ArgumentNullException(nameof(FreightConfig)));
            services.Configure<MessageConfig>(Configuration.GetSection("Message") ?? throw new ArgumentNullException(nameof(MessageConfig)));
            services.Configure<TrackApiConfig>(Configuration.GetSection("TrackApi") ?? throw new ArgumentNullException(nameof(TrackApiConfig)));
            services.Configure<KibanaConfig>(Configuration.GetSection("Kibana") ?? throw new ArgumentNullException(nameof(KibanaConfig)));

            #endregion

            // 注册目录浏览功能
            services.AddDirectoryBrowser();

            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "17Track's API",
                    Description = "17Track's API for Description",
                    TermsOfService = "https://www.17track.net",
                    Contact = new Contact
                    {
                        Name = "17TRACK",
                        Email = string.Empty,
                        Url = "https://www.17track.net"
                    },
                    License = new License
                    {
                        Name = "17TRACK",
                        Url = "https://www.17track.net"
                    }
                });

                // 为 Swagger JSON and UI设置xml文档注释路径
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                c.AddFluentValidationRules();

                #region Token绑定到ConfigureServices
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { AppConfig.Issuer, new string[] { } }, };
                c.AddSecurityRequirement(security);
                //方案名称“Blog.Core”可自定义，上下一致即可
                c.AddSecurityDefinition(AppConfig.Issuer, new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                #endregion
            });

            // 注册进程内事件发布订阅模式
            services.AddMediatR(typeof(OperationLogEventHandler).Assembly);

            // 注册RabbitMQManager为单例模式
            ConfigManager.Initialize(typeof(RpcRabbitConfig));
            var rpcRabbitConfig = ConfigManager.GetConfig<RpcRabbitConfig>();
            if (rpcRabbitConfig?.Hosts == null || !rpcRabbitConfig.Hosts.Any())
            {
                throw new ArgumentException($"配置:{nameof(rpcRabbitConfig)}加载失败");
            }
            rpcRabbitConfig.ApplicationName = "IMS.ClientRPC";
            services.AddSingleton(new RabbitMQManager(rpcRabbitConfig));

            // 注册 RPC Client 单例服务
            services.AddSingleton(x => x.GetRequiredService<RabbitMQManager>().CreateRpcClient<IPaymentRpcService>(60 * 1000));
            services.AddSingleton(x => x.GetRequiredService<RabbitMQManager>().CreateRpcClient<IUserAutoRegister>(10 * 1000));
            services.AddSingleton(x => x.GetRequiredService<RabbitMQManager>().CreateRpcClient<IDeleteUserRpcService>(3 * 60 * 1000));

            // 注册第三方ioc + 默认启用方法级别的aop
            var serviceContainer = services.ToServiceContainer();
            var serviceResolver = serviceContainer.Build();
            // 启动服务定位器模式
            GlobalServiceProvider.Current = serviceResolver;
            return serviceResolver;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // 注册应用退出事件
            //applicationLifetime.ApplicationStopping.Register(NLog.LogManager.Shutdown);

            // 使用 Forwarded Headers 传递请求相关参数 例如: x-request-ip 真实ip地址
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpMethodOverride();

            // 注册 Basic 基本身份认证请求管道
            app.UseMiddleware<BasicAuthMiddleware>("17track.com");

            // 启用静态文件浏览
            app.UseStaticFiles();

            // 增加应用程序日志访问入口
            var provider = new FileExtensionContentTypeProvider
            {
                Mappings = { [".log"] = "text/plain", [".log"] = "text/plain;charset=utf-8" }
            };
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, "Log")),
                RequestPath = new PathString("/log"),
                ContentTypeProvider = provider,
                DefaultContentType = "application/x-msdownload", // 设置未识别的MIME类型一个默认z值
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(AppContext.BaseDirectory, "Log")),
                RequestPath = new PathString("/Log")
            });

            // 增加对账单文件访问入口
            var reconcilePath = Path.Combine(env.ContentRootPath, "uploadReconcile");
            if (!Directory.Exists(reconcilePath))
            {
                Directory.CreateDirectory(reconcilePath);
            }
            var providerJson = new FileExtensionContentTypeProvider
            {
                Mappings = { [".json"] = "application/json", [".log"] = "application/json;charset=utf-8" }
            };
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(reconcilePath),
                RequestPath = new PathString("/reconcile"),
                ContentTypeProvider = providerJson,
                DefaultContentType = "application/x-msdownload", // 设置未识别的MIME类型一个默认z值
            });
            app.UseDirectoryBrowser(new DirectoryBrowserOptions
            {
                FileProvider = new PhysicalFileProvider(reconcilePath),
                RequestPath = new PathString("/reconcile")
            });

            // 启用Cookie策略
            app.UseCookiePolicy();

            // 使用自定义特殊状态码下流逻辑
            app.UseStatusCodePages(new StatusCodePagesOptions()
            {
                HandleAsync = context =>
                {
                    // 如果是普通mvc请求处理
                    if (!context.HttpContext.Request.Path.StartsWithSegments("/api") &&
                        !context.HttpContext.Request.Path.StartsWithSegments("/reconcile"))
                    {
                        if (context.HttpContext.Request.Method.ToLower() == "get")
                        {
                            context.HttpContext.Response.Redirect($"/home/{context.HttpContext.Response.StatusCode}");
                            return Task.CompletedTask;
                        }

                        var apiResult = new ApiResult
                        {
                            Success = false,
                            Msg = ((HttpStatusCode)context.HttpContext.Response.StatusCode).GetDescription()
                        };

                        if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                        {
                            apiResult.Msg = "当前用户身份认证失败";
                        }

                        if (context.HttpContext.Response.StatusCode == (int)HttpStatusCode.Forbidden)
                        {
                            apiResult.Msg = "当前用户授权失败";
                        }

                        context.HttpContext.Response.Headers.Remove("Location");
                        context.HttpContext.Response.StatusCode = 200;
                        context.HttpContext.Response.ContentType = "application/json";
                        return context.HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(apiResult, new JsonSerializerSettings
                        {
                            Formatting = Formatting.Indented,
                            ContractResolver = new DefaultContractResolver
                            {
                                NamingStrategy = new CamelCaseNamingStrategy()
                            }
                        }));
                    }
                    // 如果是Api请求返回原始状态码即可
                    return Task.CompletedTask;
                }
            });

            // 启用回话功能
            app.UseSession();

            // 启用身份认证
            app.UseAuthentication();

            // 启用Hangfire定时任务面板
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                // 面板的名字
                DashboardTitle = "IMS定时任务",
                // 禁止展示存储连接
                DisplayStorageConnectionString = false,
                // 设置身份验证
                Authorization = new[] { new HangfireDashboardAuthorizationFilter() },
                // 设置为10s页面状态刷新
                StatsPollingInterval = 10000
            });

            // 使用MVC路由
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "17Track API V1");
            });
        }
    }
}
