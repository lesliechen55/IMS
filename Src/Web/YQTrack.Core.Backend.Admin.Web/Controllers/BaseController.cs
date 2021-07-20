using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [PermissionCheckActionFilter]
    public abstract class BaseController : Controller
    {
        protected virtual FileContentResult FileExcel(byte[] bytes, string fileName = null)
        {
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", string.IsNullOrWhiteSpace(fileName) ? Path.ChangeExtension(Path.GetRandomFileName(), "xlsx") : fileName.Trim());
        }

        protected virtual FileContentResult FileZip(byte[] bytes, string fileName = null)
        {
            return File(bytes, "application/x-zip-compressed", string.IsNullOrWhiteSpace(fileName) ? Path.ChangeExtension(Path.GetRandomFileName(), "zip") : fileName.Trim());
        }

        protected virtual FileContentResult FileHtml(byte[] bytes, string fileName = null)
        {
            return File(bytes, "text/html", string.IsNullOrWhiteSpace(fileName) ? Path.ChangeExtension(Path.GetRandomFileName(), "html") : fileName.Trim());
        }

        protected virtual FileContentResult FileImage(byte[] bytes)
        {
            return File(bytes, "image/jpeg");
        }

        protected virtual ApiJsonResult ApiJson()
        {
            return new ApiJsonResult(new ApiResult());
        }

        protected virtual ApiJsonResult ApiJson(ApiResult apiResult)
        {
            return new ApiJsonResult(apiResult);
        }

        /// <summary>
        /// 方便外部调用的ApiJson方法翻版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual ApiJsonResult MyJson<T>(T data)
        {
            return new ApiJsonResult(new ApiResult<T>(data));
        }

        /// <summary>
        /// 方便外部调用的分页方法翻版
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        protected virtual ApiJsonResult MyPageJson<T>(IEnumerable<T> data, int count) where T : class, new()
        {
            return new ApiJsonResult(new PageResponse<T>(data, count));
        }

        protected virtual ApiJsonResult ApiJson(ApiResult apiResult, JsonSerializerSettings jsonSerializerSettings)
        {
            return new ApiJsonResult(apiResult, jsonSerializerSettings);
        }

        protected LoginManager LoginManager
        {
            get
            {
                var userClaims = HttpContext.User.Claims.ToArray();

                if (userClaims.Any(x => x.Type == ClaimTypes.Sid) && userClaims.Any(x => x.Type == ClaimTypes.Name) && userClaims.Any(x => x.Type == ClaimTypes.NameIdentifier))
                {
                    return new LoginManager()
                    {
                        Id = int.Parse(userClaims.Single(x => x.Type == ClaimTypes.Sid).Value),
                        NickName = userClaims.Single(x => x.Type == ClaimTypes.Name).Value,
                        Account = userClaims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value
                    };
                }
                throw new BusinessException("当前登录用户没有登录或者信息被篡改,无法识别");
            }
        }
    }
}