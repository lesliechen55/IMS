using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Models.Request;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IManagerService _managerService;
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AccountController(IManagerService managerService,
                                 IMapper mapper,
                                 IHostingEnvironment hostingEnvironment)
        {
            _managerService = managerService;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var output = await _managerService.GetByIdAsync(LoginManager.Id);
            var response = _mapper.Map<ManagerPageDataResponse>(output);
            return View(response);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> ChangePassword(ChangePwdRequest request)
        {
            await _managerService.ChangePwdAsync(LoginManager.Id, request.NewPassword, request.OldPassword);
            return ApiJson();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> UpdateNickName([Required, StringLength(16, MinimumLength = 1)]string nickName)
        {
            await _managerService.UpdateNickNameAsync(LoginManager.Id, nickName);
            return ApiJson();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> UpdateAvatar(UpdateAvatarRequest request)
        {
            var upload = Path.Combine(_hostingEnvironment.WebRootPath, "uploadAvatar");
            if (!Directory.Exists(upload))
            {
                Directory.CreateDirectory(upload);
            }
            var filePath = Path.Combine(upload, $"{LoginManager.Id}.jpg");
            using (var stream = System.IO.File.OpenWrite(filePath))
            {
                await request.FormFile.CopyToAsync(stream);
            }
            var virtualFilePath = $"/uploadAvatar/{LoginManager.Id}.jpg";
            await _managerService.UpdateAvatarAsync(LoginManager.Id, virtualFilePath);
            return ApiJson();
        }
    }
}