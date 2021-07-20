using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Service;
using YQTrack.Log;

namespace YQTrack.Core.Backend.Admin.WebCore
{
    /// <summary>
    /// 添加身份识别权限认证过滤器
    /// </summary>
    public class PermissionCheckActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var descriptor = context.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            // 控制器AllowAnonymousAttribute标记检查
            var anonymousAttributes = descriptor.ControllerTypeInfo.GetCustomAttributes<AllowAnonymousAttribute>();

            // 行为AllowAnonymousAttribute标记检查
            var allowAnonymousAttributes = descriptor.MethodInfo.GetCustomAttributes<AllowAnonymousAttribute>();
            if (allowAnonymousAttributes.Any() || anonymousAttributes.Any())
            {
                return;
            }

            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<PermissionCheckActionFilter>>();

            // 登陆身份检查
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                if (context.HttpContext.Request.Method.ToLower() == "get")
                {
                    context.Result = new UnauthorizedResult();
                }
                else
                {
                    context.Result = new JsonResult(new ApiResult { Success = false, Msg = "当前用户身份认证失败" });
                }
                return;
            }

            // 加载当前用户的所有角色的权限并集与当前请求资源的权限代码坐鉴权处理
            var sId = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Sid);
            if (sId == null)
            {
                throw new ArgumentNullException(nameof(sId));
            }

            var managerId = Convert.ToInt32(sId.Value);

            // 权限检查
            var permissionCodes = new List<string>();

            var homeService = context.HttpContext.RequestServices.GetRequiredService<IHomeService>();
            var existPermissions = homeService.GetExistPermissionList(managerId);

            bool authorize = false;
            object action = string.Empty;
            if (context.ActionArguments.TryGetValue("permissionCode", out action))
            {
                authorize = existPermissions.Any(c => c.Equals(action.ToString(), StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                var permissionCodeAttribute = descriptor.MethodInfo.GetCustomAttribute<PermissionCodeAttribute>();

                var areaName = descriptor.RouteValues["area"] ?? string.Empty;
                var controllerName = descriptor.ControllerName;
                var actionName = descriptor.ActionName;
                if (permissionCodeAttribute != null)
                {
                    permissionCodes = permissionCodeAttribute.PermissionNames
                        .Select(s =>
                            areaName.IsNullOrEmpty()
                            ? $"{controllerName}_{s}"
                            : $"{areaName}_{controllerName}_{s}"
                        ).ToList();
                }
                else
                {
                    permissionCodes.Add(areaName.IsNullOrEmpty()
                        ? $"{controllerName}_{actionName}"
                        : $"{areaName}_{controllerName}_{actionName}");
                }
                authorize = permissionCodes.Any(x => existPermissions.Any(c => c.Equals(x, StringComparison.InvariantCultureIgnoreCase)));
            }

            if (!authorize)
            {
                logger.LogWarning(JsonConvert.SerializeObject(new
                {
                    Url = context.HttpContext.Request.GetDisplayUrl(),
                    ErrorMsg = "当前用户授权失败,请联系系统管理员授权",
                    PermissionCodes = permissionCodes,
                    User = context.HttpContext.User.Claims.Select(x => new { x.Type, x.Value })
                }));

                LogHelper.LogObj(new LogDefinition(Log.LogLevel.Warn, "用户授权失败"),new
                {
                    Url = context.HttpContext.Request.GetDisplayUrl(),
                    ErrorMsg = "当前用户授权失败,请联系系统管理员授权",
                    PermissionCodes = permissionCodes,
                    User = context.HttpContext.User.Claims.Select(x => new { x.Type, x.Value })
                });

                if (context.HttpContext.Request.Method.ToLower() == "get")
                {
                    context.Result = new ForbidResult();
                }
                else
                {
                    context.Result = new JsonResult(new ApiResult { Success = false, Msg = "当前用户授权失败" });
                }

                return;
            }

            base.OnActionExecuting(context);

        }
    }
}