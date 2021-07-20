using System;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.Injector;
using Microsoft.Extensions.Logging;

namespace YQTrack.Core.Backend.Admin.Core.Interceptor
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class TestDebugInterceptorAttribute : AbstractInterceptorAttribute
    {
        [FromContainer]
        public ILogger<TestDebugInterceptorAttribute> Logger { get; set; }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            Logger.LogInformation("Before service call");
            await next(context);
            Logger.LogInformation("After service call");
        }
    }
}