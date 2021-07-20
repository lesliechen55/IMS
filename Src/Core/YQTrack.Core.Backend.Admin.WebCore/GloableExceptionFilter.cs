using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using YQTrack.Log;

namespace YQTrack.Core.Backend.Admin.WebCore
{
    /// <summary>
    /// 全局异常拦截器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GlobalExceptionFilter(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Error, "GlobalException"),
                context.Exception.InnerException ?? context.Exception,
                new { Message = context.Exception.InnerException != null ? context.Exception.InnerException.Message : context.Exception.Message });
            context.Result = _hostingEnvironment.IsDevelopment()
                ? new JsonResult(new ApiResult { Success = false, Msg = context.Exception.ToString() })
                : new JsonResult(new ApiResult { Success = false, Msg = context.Exception.InnerException == null ? context.Exception.Message : context.Exception.InnerException.Message });
            context.ExceptionHandled = true;
        }
    }
}