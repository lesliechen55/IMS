using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using AspectCore.Injector;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.MediatR;

namespace YQTrack.Core.Backend.Admin.Data.Interceptor
{
    /// <summary>
    /// 业务操作跟踪拦截器(加强版)
    /// 通过Mediator发送进程内的Event事件,提高逻辑分离且提升性能
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class OperationTracePlusAttribute : AbstractInterceptorAttribute
    {
        private readonly string _desc;
        private readonly OperationType _type;

        /// <summary>
        /// 业务操作跟踪拦截器
        /// </summary>
        /// <param name="desc">业务操作描述</param>
        /// <param name="type">操作类型</param>
        public OperationTracePlusAttribute(string desc, OperationType type)
        {
            if (desc.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(desc));
            }
            _desc = desc;
            _type = type;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            await next(context);
            var httpContextAccessor = context.ServiceProvider.ResolveRequired<IHttpContextAccessor>();
            var userClaims = httpContextAccessor.HttpContext?.User?.Claims.ToList();
            if (userClaims != null && userClaims.Any(x => x.Type == ClaimTypes.Sid) && userClaims.Any(x => x.Type == ClaimTypes.Name) && userClaims.Any(x => x.Type == ClaimTypes.NameIdentifier))
            {
                var operatorId = int.Parse(userClaims.Single(x => x.Type == ClaimTypes.Sid).Value);
                var parameters = context.GetParameters().Select(x => new { x.Name, x.Value, Type = x.Type.ToString() });
                var @event = new OperationLogEvent
                {
                    FAccount = userClaims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value,
                    FCreatedBy = operatorId,
                    FDesc = _desc,
                    //FIp = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    FIp = httpContextAccessor.GetReadIpAddress(),
                    FOperatorId = operatorId,
                    FMethod = $"{context.ServiceMethod.DeclaringType.FullName}_{context.ProxyMethod.Name}",
                    FNickName = userClaims.Single(x => x.Type == ClaimTypes.Name).Value,
                    FOperationType = _type,
                    FParameter = JsonConvert.SerializeObject(parameters)
                };
                var mediator = context.ServiceProvider.ResolveRequired<IMediator>();
                await mediator.Publish(@event);
            }
        }
    }
}