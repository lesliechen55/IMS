using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Service;
using YQTrack.Core.Backend.Admin.Web.Models;
using YQTrack.Core.Backend.Admin.Web.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Models.Response;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHomeService _homeService;
        private readonly IMapper _mapper;
        private readonly ISession _session;

        public HomeController(IHttpContextAccessor httpContextAccessor,
                                IHomeService homeService,
                                IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _homeService = homeService;
            _mapper = mapper;
            _session = httpContextAccessor.HttpContext.Session;
        }

        /// <summary>
        /// 渲染主页且包含当前管理员的菜单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (topKeyAndNameDic, defaultSelectedTopKey, avatar) = await _homeService.GetTopKeyAndNameDicAsync(LoginManager.Id);

            return View(new IndexResponse
            {
                TopKeyAndNameDic = topKeyAndNameDic,
                DefaultSelectedTopKey = defaultSelectedTopKey,
                BackendMainUrl = Url.Action("Main", "Home"),
                Avatar = avatar
            });
        }

        /// <summary>
        /// 获取菜单以get形式的json数据返回
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetMenuData()
        {
            var output = await _homeService.GetMenuAsync(LoginManager.Id);
            var response = _mapper.Map<Dictionary<string, MenuResponse[]>>(output);
            return Json(response);
        }

        [HttpGet]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> Main()
        {
            var (totalUser, totalRole, totalPermission, lastLoginTime) = await _homeService.GetMainDataAsync(LoginManager.Id);
            return View(new MainResponse
            {
                TotalManager = totalUser,
                TotalRole = totalRole,
                TotalPermission = totalPermission,
                LastLoginTime = lastLoginTime.ToLocalTime()
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var code = _session.GetString("code");
            if (code.IsNullOrEmpty())
            {
                return ApiJson(new ApiResult { Success = false, Msg = "验证码已过期" });
            }
            if (code != request.Code)
            {
                return ApiJson(new ApiResult { Success = false, Msg = "验证码错误" });
            }
            var (userId, account, nickName) = await _homeService.LoginAsync(request.Account, request.Password, _httpContextAccessor.GetReadIpAddress(), Request.Headers["User-Agent"].ToString());
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid,userId.ToString(),ClaimValueTypes.Integer32),
                new Claim(ClaimTypes.Name,nickName,ClaimValueTypes.String),
                new Claim(ClaimTypes.NameIdentifier,account,ClaimValueTypes.String)
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties
            {
                IsPersistent = request.RememberMe,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            });
            return Json(new ApiResult());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Home");
        }

        /// <summary>
        /// 生成随机验证码数字字符串——4位数
        /// </summary>
        /// <returns></returns>
        private static string RandomNum()
        {
            var random = new Random(Guid.NewGuid().GetHashCode());
            var intString = random.Next(1000, 9999).ToString();
            return intString;
        }

        private static int[] RandomPoint()
        {
            var intArray = new int[6];
            for (var i = 0; i < 6; i += 2)
            {
                var random = new Random(Guid.NewGuid().GetHashCode());
                switch (i)
                {
                    case 0:
                        intArray[i] = random.Next(0, 10);
                        break;
                    case 2:
                        intArray[i] = random.Next(45, 55);
                        break;
                    case 4:
                        intArray[i] = random.Next(90, 100);
                        break;
                }
            }
            for (var i = 1; i < 6; i += 2)
            {
                var random = new Random(Guid.NewGuid().GetHashCode());
                intArray[i] = random.Next(0, 42);
            }
            return intArray;
        }

        private byte[] CreateImage()
        {
            //设置图片大小
            Image image = new Bitmap(116, 36);
            //设置画笔在哪一张图片上画图
            Graphics graph = Graphics.FromImage(image);
            //背景色
            graph.Clear(Color.White);
            //笔刷
            Pen pen = new Pen(Brushes.Black, 2);
            for (var i = 0; i < 4; i++)
            {
                var points = RandomPoint();
                //画一条曲线
                graph.DrawCurve(pen, new Point[] {
                    new Point(points[0], points[1]),
                    new Point(points[2], points[3]),
                    new Point(points[4], points[5])
                });
            }
            //画一条直线
            graph.DrawLines(pen, new Point[] { new Point(10, 10), new Point(90, 40) });

            var randomNum = RandomNum();
            _session.SetString("code", randomNum);

            //画数字
            graph.DrawString(randomNum, new Font(new FontFamily("Microsoft YaHei"), 20, FontStyle.Bold),
                Brushes.Black, new PointF(10, 0));
            //内存流
            var ms = new MemoryStream();
            //把图片存进内存流
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //获取内存流的byte数组
            var buf = ms.GetBuffer();
            return buf;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetValidateCode()
        {
            byte[] buf = CreateImage();
            return File(buf, "image/png");
        }

        [Route("/home/404")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult MyNotFound()
        {
            return View("NotFound");
        }

        [Route("/home/403")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult UnAuthorized()
        {
            return View("Forbid");
        }

        [Route("/home/401")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult UnAuthenticate()
        {
            return RedirectToAction("Login", "Home");
        }

        [Route("/home/forbid")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult MyForbid()
        {
            return View("forbid");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ValidateParameterFailed(string error)
        {
            return View(new ApiResult { Msg = error, Success = false });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
