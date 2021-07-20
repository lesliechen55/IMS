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
    public class RoleController : BaseAdminController
    {
        private readonly IMapper _mapper;
        private readonly IRoleService _roleService;

        public RoleController(IMapper mapper, IRoleService roleService)
        {
            _mapper = mapper;
            _roleService = roleService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetPageData(RolePageDataRequest request)
        {
            var (outputs, total) = await _roleService.GetPageDataAsync(request.Name, request.Page, request.Limit);
            var response = _mapper.Map<IEnumerable<RolePageDataResponse>>(outputs);
            return Json(new PageResponse<RolePageDataResponse> { Data = response, Count = total });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult Add()
        {
            return View(new IframeTransferData());
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add(RoleAddRequest request)
        {
            var roleAddInput = _mapper.Map<RoleAddInput>(request);
            await _roleService.AddAsync(roleAddInput);
            return ApiJson();
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([NotEmpty, FromQuery]int roleId)
        {
            var (output, roleUserNameList) = await _roleService.GetByIdAsync(roleId);
            var response = _mapper.Map<RolePageDataResponse>(output);
            response.RoleAccounts = roleUserNameList.Any() ? string.Join(" , ", roleUserNameList) : "无";
            return View(new IframeTransferData<RolePageDataResponse>
            {
                Id = roleId.ToString(),
                Data = response
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(RoleEditRequest request)
        {
            await _roleService.EditAsync(request.Id, request.Name, request.IsActive, request.Remark, LoginManager.Id);
            return ApiJson();
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult QueryPermissionList([NotEmpty, FromQuery]int roleId)
        {
            return View(roleId);
        }

        [HttpPost]
        [ActionName(nameof(QueryPermissionList))]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(QueryPermissionList))]
        public async Task<IActionResult> QueryPermissionListAsync([NotEmpty]int roleId)
        {
            var outputs = await _roleService.GetRolePermissionListAsync(roleId);
            var responses = _mapper.Map<IEnumerable<RolePermissionResponse>>(outputs).ToList();
            return Json(new RolePermissionTreeResponse
            {
                Data = responses.Select(x => new RolePermissionTreeItemResponse { Id = x.Id, Name = x.Name, PId = x.ParentId ?? 0 }),
                Count = responses.Count,
                CheckedIdList = responses.Where(x => x.IsSelect).Select(x => x.Id).ToArray()
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> SetPermissionList([FromForm]RoleSetPermissionRequest request)
        {
            await _roleService.SetPermissionListAsync(request.Id, request.PermissionIdList);
            return Json(new ApiResult());
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Delete([NotEmpty] int id)
        {
            await _roleService.DeleteAsync(id);
            return ApiJson();
        }
    }
}