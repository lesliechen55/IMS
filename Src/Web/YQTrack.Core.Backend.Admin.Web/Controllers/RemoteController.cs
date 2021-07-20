using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Service;
using YQTrack.Core.Backend.Admin.Web.Models.Request;
using YQTrack.Core.Backend.Admin.WebCore;
using YQTrack.Log;

namespace YQTrack.Core.Backend.Admin.Web.Controllers
{
    public class RemoteController : BaseController
    {
        private readonly IHomeService _homeService;
        private readonly ILogger<RemoteController> _logger;

        public RemoteController(IHomeService homeService, ILogger<RemoteController> logger)
        {
            _homeService = homeService;
            _logger = logger;
        }

        /// <summary>
        /// 提供远程API请求IMS检查指定用户是否具有相应权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ModelStateValidationFilter]
        public async Task<IActionResult> CheckPermission(PermissionCheckRequest request)
        {
            var (userId, account, _) = await _homeService.LoginAsync(request.Account, request.Password, request.Ip, request.UserAgent, request.PlatForm, true);
            var existPermissions = _homeService.GetExistPermissionList(userId);
            var authorize = request.PermissionCodeList.Any(x => existPermissions.Any(c => c.Equals(x, StringComparison.InvariantCultureIgnoreCase)));
            if (!authorize)
            {
                _logger.LogWarning(JsonConvert.SerializeObject(new
                {
                    Url = HttpContext.Request.GetDisplayUrl(),
                    ErrorMsg = "当前用户授权失败,请联系系统管理员授权",
                    PermissionCodes = request.PermissionCodeList,
                    User = HttpContext.User.Claims.Select(x => new { x.Type, x.Value })
                }));

                LogHelper.LogObj(new LogDefinition(YQTrack.Log.LogLevel.Warn, "CheckPermission授权失败"), new
                {
                    Url = HttpContext.Request.GetDisplayUrl(),
                    ErrorMsg = "当前用户授权失败,请联系系统管理员授权",
                    PermissionCodes = request.PermissionCodeList,
                    User = HttpContext.User.Claims.Select(x => new { x.Type, x.Value })
                });

                return Json(new ApiResult
                {
                    Success = false,
                    Msg = $"当前用户:{account},授权处理器权限代码:{JsonConvert.SerializeObject(request.PermissionCodeList)} 失败,请检查是否权限角色配置正确!"
                });
            }
            return ApiJson();
        }
    }
}