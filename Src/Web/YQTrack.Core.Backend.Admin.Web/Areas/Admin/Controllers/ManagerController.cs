using System.Collections.Generic;
using System.Linq;
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
    public class ManagerController : BaseAdminController
    {
        private readonly IMapper _mapper;
        private readonly IManagerService _managerService;

        public ManagerController(IMapper mapper, IManagerService managerService)
        {
            _mapper = mapper;
            _managerService = managerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetPageData(ManagerPageDataRequest request)
        {
            var input = _mapper.Map<ManagerPageDataInput>(request);
            var (outputs, total) = await _managerService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<ManagerPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<ManagerPageDataResponse>
            {
                Data = data,
                Count = total
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult Add()
        {
            return View(new IframeTransferData());
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add(ManagerAddRequest request)
        {
            var input = _mapper.Map<ManagerAddInput>(request);
            await _managerService.AddAsync(input, LoginManager.Id);
            return ApiJson();
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([NotEmpty, FromQuery]int id)
        {
            var output = await _managerService.GetByIdAsync(id);
            var response = _mapper.Map<ManagerPageDataResponse>(output);
            return View(new IframeTransferData<ManagerPageDataResponse>
            {
                Id = id.ToString(),
                Data = response
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(ManagerEditRequest request)
        {
            await _managerService.EditAsync(request.Id, request.NickName, request.Password, request.IsLock, LoginManager.Id, request.Remark);
            return ApiJson();
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult GetRoleList([NotEmpty]int userId)
        {
            return View(userId);
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(GetRoleList))]
        public async Task<IActionResult> GetRoleData([NotEmpty]int userId)
        {
            var outputs = await _managerService.GetRoleListAsync(userId);
            var response = _mapper.Map<IEnumerable<ManagerRoleResponse>>(outputs).ToList();
            return ApiJson(new PageResponse<ManagerRoleResponse>
            {
                Data = response,
                Count = response.Count
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> SetRoleList([FromForm]ManagerSetRoleRequest request)
        {
            await _managerService.SetRoleListAsync(request.Id, request.RoleIdList);
            return ApiJson();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Delete([NotEmpty] int id)
        {
            await _managerService.DeleteAsync(id);
            return ApiJson();
        }
    }
}