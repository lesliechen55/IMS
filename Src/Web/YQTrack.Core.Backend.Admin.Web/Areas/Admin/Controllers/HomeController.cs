using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.DTO.Input;
using YQTrack.Core.Backend.Admin.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        private readonly IHomeService _homeService;
        private readonly IMapper _mapper;

        public HomeController(IHomeService homeService,
                              IMapper mapper)
        {
            _homeService = homeService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> LoginLog()
        {
            var hasSuperRole = await _homeService.HasSuperRoleAsync(LoginManager.Id);
            return View(hasSuperRole);
        }

        [HttpPost]
        [PermissionCode(nameof(LoginLog))]
        public async Task<IActionResult> GetLoginLogPageData(LoginLogPageDataRequest request)
        {
            var input = _mapper.Map<LoginLogPageDataInput>(request);
            // 如果不是拥有超级角色只能查看自身日志记录
            var hasSuperRole = await _homeService.HasSuperRoleAsync(LoginManager.Id);
            if (!hasSuperRole)
            {
                input.Account = LoginManager.Account;
                input.NickName = LoginManager.NickName;
            }
            var (outputs, total) = await _homeService.GetLoginLogPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<LoginLogPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<LoginLogPageDataResponse>
            {
                Data = responses,
                Count = total
            });
        }

        [HttpGet]
        public async Task<IActionResult> OperationLog()
        {
            var hasSuperRole = await _homeService.HasSuperRoleAsync(LoginManager.Id);
            return View(hasSuperRole);
        }

        [HttpPost]
        [PermissionCode(nameof(OperationLog))]
        [ModelStateValidationFilter]
        [ResultEncodeFilter]
        public async Task<IActionResult> GetOperationLogPageData(OperationLogPageDataRequest request)
        {
            var input = _mapper.Map<OperationLogPageDataInput>(request);
            // 如果不是拥有超级角色只能查看自身日志记录
            var hasSuperRole = await _homeService.HasSuperRoleAsync(LoginManager.Id);
            if (!hasSuperRole)
            {
                input.Account = LoginManager.Account;
            }
            var (outputs, total) = await _homeService.GetOperationLogPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<OperationLogPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<OperationLogPageDataResponse>
            {
                Data = responses,
                Count = total
            });
        }
    }
}