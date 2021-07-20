
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.WebCore
{
    /// <summary>
    /// 添加文本结果编码过滤器
    /// </summary>
    public class ResultEncodeFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is JsonResult result)
            {
                if (result.Value != null)
                {
                    SetPropertyValue(result.Value);
                    //context.Result = result;
                }
            }
            base.OnResultExecuting(context);
        }
        /// <summary>
        /// 递归字段处理字符串编码
        /// </summary>
        /// <param name="obj"></param>
        private void SetPropertyValue(object obj)
        {
            Type objType = obj.GetType();
            if (obj is string strValue)
            {
                if (!strValue.IsNullOrWhiteSpace())
                {
                    obj = HttpUtility.HtmlEncode(strValue);
                }
            }
            else if (typeof(IEnumerable).IsAssignableFrom(objType))
            {
                if (TypeHelper.GetAnyElementType(objType).IsValueType)
                {
                    return;
                }
                foreach (var item in (IEnumerable<object>)obj)
                {
                    SetPropertyValue(item);
                }
            }
            else if (objType.IsClass)
            {
                IEnumerable<PropertyInfo> propertyInfos = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty).Where(w => w.CanWrite && (!w.PropertyType.IsValueType));
                foreach (var propertyInfo in propertyInfos)
                {
                    if (propertyInfo.PropertyType == typeof(string))
                    {
                        var str = propertyInfo.GetValue(obj) as string;
                        if (!str.IsNullOrWhiteSpace())
                        {
                            propertyInfo.SetValue(obj, HttpUtility.HtmlEncode(str.Trim()));
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
                    else if (!propertyInfo.PropertyType.IsValueType)
                    {
                        SetPropertyValue(propertyInfo.GetValue(obj));
                    }
                }
            }
        }
    }
}