using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Log;

namespace YQTrack.Core.Backend.Admin.WebCore
{
    /// <summary>
    /// 添加模型验证过滤器
    /// </summary>
    public class ModelStateValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<ModelStateValidationFilter>>();

                var descriptor = (Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor;

                var msg = context.ModelState.GetAllErrorMsg();

                logger.LogWarning(JsonConvert.SerializeObject(new
                {
                    AreaName = descriptor.RouteValues["area"],
                    descriptor.ControllerName,
                    descriptor.ActionName,
                    Args = JsonConvert.SerializeObject(context.ActionArguments),
                    ModelStateErrors = msg
                }));

                LogHelper.LogObj(new LogDefinition(Log.LogLevel.Warn, "MVC模型验证错误"),new
                {
                    AreaName = descriptor.RouteValues["area"],
                    descriptor.ControllerName,
                    descriptor.ActionName,
                    Args = JsonConvert.SerializeObject(context.ActionArguments),
                    ModelStateErrors = msg
                });

                // 去除重复错误
                if (msg.Split(';').Any())
                {
                    msg = string.Join(';', msg.Split(';').Distinct());
                }

                if (context.HttpContext.Request.Method.ToLower() == "get")
                {
                    context.Result = new RedirectToActionResult("ValidateParameterFailed", "Home", new { Error = msg });
                }
                else
                {
                    context.Result = new JsonResult(new ApiResult { Msg = msg, Success = false });
                }
                return;
            }
            for (int i = 0; i < context.ActionArguments.Values.Count; i++)
            {
                KeyValuePair<string, object> pair = context.ActionArguments.Skip(i).Take(1).FirstOrDefault();
                if (pair.Value != null)
                {
                    context.ActionArguments[pair.Key] = SetPropertyValue(pair.Value);
                }
            }
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 递归字段处理字符串去空
        /// </summary>
        /// <param name="obj"></param>
        private static object SetPropertyValue(object obj)
        {
            Type objType = obj.GetType();
            if (obj is string strValue)
            {
                if (!strValue.IsNullOrWhiteSpace())
                {
                    return strValue.Trim();
                }
            }
            else if (typeof(IEnumerable).IsAssignableFrom(objType))
            {
                if (TypeHelper.GetAnyElementType(objType).IsValueType)
                {
                    return obj;
                }
                foreach (var item in (IEnumerable<object>)obj)
                {
                    return SetPropertyValue(item);
                }
            }
            else if (objType.IsClass)
            {
                IEnumerable<PropertyInfo> propertyInfos = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty).Where(w => w.CanWrite && (!w.PropertyType.IsValueType));
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        strValue = propertyInfo.GetValue(obj) as string;
                        if (!strValue.IsNullOrWhiteSpace())
                        {
                            propertyInfo.SetValue(obj, strValue.Trim());
                        }
                    }
                    else if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        if (TypeHelper.GetAnyElementType(propertyInfo.PropertyType).IsValueType)
                        {
                            continue;
                        }
                        var o = propertyInfo.GetValue(obj);
                        if (o == null)
                        {
                            continue;
                        }
                        foreach (var item in (IEnumerable<object>)o)
                        {
                            SetPropertyValue(item);
                        }
                    }
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        SetPropertyValue(propertyInfo.GetValue(obj));
                    }
                }
            }
            return obj;
        }
    }
}