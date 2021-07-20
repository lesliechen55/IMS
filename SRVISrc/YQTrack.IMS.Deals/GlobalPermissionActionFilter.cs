using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using YQTrackV6.Log;

namespace YQTrack.IMS.Deals
{
    /// <summary>
    /// IMS权限过滤检查器(当网络错误或者授权失败的情况将会以异常的形式返回给应用程序,需要自行处理异常)
    /// </summary>
    public class GlobalPermissionActionFilter : ActionFilterAttribute
    {
        private readonly string _account;
        private readonly string _password;
        private readonly string _imsPermissionCheckUrl;

        private static readonly LogDefinition ErrorLogDefinition = new LogDefinition(LogLevel.Error, nameof(GlobalPermissionActionFilter));
        private static readonly LogDefinition InfoLogDefinition = new LogDefinition(LogLevel.Info, nameof(GlobalPermissionActionFilter));

        private static readonly HttpClient HttpClient = new HttpClient()
        {
            Timeout = TimeSpan.FromSeconds(3)
        };

        /// <summary>
        /// IMS权限过滤检查器(当网络错误或者授权失败的情况将会以异常的形式返回给应用程序,需要自行处理异常)
        /// </summary>
        /// <param name="account">ims分配的账号</param>
        /// <param name="password">ims账号密码</param>
        /// <param name="imsPermissionCheckUrl">ims系统远程权限检查地址</param>
        public GlobalPermissionActionFilter(string account, string password, string imsPermissionCheckUrl)
        {
            if (string.IsNullOrWhiteSpace(account))
            {
                LogHelper.LogObj(ErrorLogDefinition, new { Msg = $"错误:参数{nameof(account)}不能为空" });
                throw new ArgumentNullException(nameof(account), $"错误:参数{nameof(account)}不能为空");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                LogHelper.LogObj(ErrorLogDefinition, new { Msg = $"错误:参数{nameof(password)}不能为空" });
                throw new ArgumentNullException(nameof(password), $"错误:参数{nameof(password)}不能为空");
            }
            if (string.IsNullOrWhiteSpace(imsPermissionCheckUrl))
            {
                LogHelper.LogObj(ErrorLogDefinition, new { Msg = $"错误:参数{nameof(imsPermissionCheckUrl)}不能为空" });
                throw new ArgumentNullException(nameof(imsPermissionCheckUrl), $"错误:参数{nameof(imsPermissionCheckUrl)}不能为空");
            }
            _account = account;
            _password = password;
            _imsPermissionCheckUrl = imsPermissionCheckUrl;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException(nameof(filterContext));
            }

            var actionDescriptor = filterContext.ActionDescriptor;
            if (actionDescriptor == null)
            {
                throw new ArgumentNullException(nameof(actionDescriptor));
            }

            // 控制器AllowAnonymousAttribute标记检查
            var controllerAllowAnonymousAttributes = actionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);

            // 行为AllowAnonymousAttribute标记检查
            var actionAnonymousAttributes = actionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);
            if (actionAnonymousAttributes.Any() || controllerAllowAnonymousAttributes.Any())
            {
                return;
            }

            // 获取当前访问Action的PermissionCode集合
            var permissionCodes = new List<string>();
            if (actionDescriptor.GetCustomAttributes(typeof(PermissionCodeAttribute), false) is PermissionCodeAttribute[] permissionCodeAttributes && permissionCodeAttributes.Length == 1)
            {
                permissionCodes = permissionCodeAttributes[0].PermissionNames.ToList();
            }
            else
            {
                permissionCodes.Add($"{actionDescriptor.ControllerDescriptor.ControllerName}_{actionDescriptor.ActionName}");
            }

            // 远程访问IMS检查是否当前用户允许访问指定权限代码对应的资源Url
            var parameters = new[]
            {
                new KeyValuePair<string, string>("account", _account),
                new KeyValuePair<string, string>("password", _password),
                new KeyValuePair<string, string>("platForm", "deals"),
                new KeyValuePair<string, string>("ip", filterContext.HttpContext.Request.UserHostAddress),
                new KeyValuePair<string, string>("userAgent", filterContext.HttpContext.Request.UserAgent),
                new KeyValuePair<string, string>("permissionCode", JsonConvert.SerializeObject(permissionCodes))
            };

            try
            {
                var response = HttpClient.PostAsync(_imsPermissionCheckUrl,
                               new FormUrlEncodedContent(parameters)).Result;

                if (!response.IsSuccessStatusCode)
                {
                    LogHelper.LogObj(ErrorLogDefinition, new { Msg = $"请求IMS网络异常:错误代码->{(int)response.StatusCode},原因:{nameof(response.ReasonPhrase)}" });
                }
                response.EnsureSuccessStatusCode();

                var result = response.Content.ReadAsStringAsync().Result;

                LogHelper.LogObj(InfoLogDefinition, new { RequestBody = parameters, ResponseContent = result });

                var apiResult = JsonConvert.DeserializeObject<ApiResult>(result);

                if (!apiResult.Success)
                {
                    if (filterContext.RequestContext.HttpContext.Request.HttpMethod.ToLower() == "get")
                    {
                        filterContext.Result = new RedirectResult("/YQServerConfig/NoPremission");
                        filterContext.HttpContext.Response.ContentEncoding = Encoding.UTF8;
                        return;
                    };
                    filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                    return;
                }
            }
            catch (Exception e)
            {
                LogHelper.LogObj(ErrorLogDefinition, e);
                throw;
            }


            base.OnActionExecuting(filterContext);
        }
    }
}
