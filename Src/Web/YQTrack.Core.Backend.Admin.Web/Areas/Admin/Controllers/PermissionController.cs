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
    public class PermissionController : BaseAdminController
    {
        private readonly IPermissionService _permissionService;
        private readonly IMapper _mapper;

        public PermissionController(IPermissionService permissionService, IMapper mapper)
        {
            _permissionService = permissionService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetAll()
        {
            var outputs = await _permissionService.GetAllAsync();
            var responses = _mapper.Map<List<PermissionResponse>>(outputs);
            return new JsonResult(new PageResponse<PermissionResponse>
            {
                Data = responses,
                Count = responses.Count
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
        public async Task<IActionResult> Add(PermissionAddRequest request)
        {
            var input = _mapper.Map<PermissionAddInput>(request);
            await _permissionService.AddAsync(input, LoginManager.Id);
            return ApiJson();
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([NotEmpty]int id)
        {
            var permissionOutput = await _permissionService.GetByIdAsync(id);
            var response = _mapper.Map<PermissionResponse>(permissionOutput);
            return View(new IframeTransferData<PermissionResponse>
            {
                Id = id.ToString(),
                Data = response
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(PermissionEditRequest request)
        {
            // ReSharper disable once PossibleInvalidOperationException
            await _permissionService.EditAsync(request.Id, request.Name, request.AreaName, request.ControllerName, request.ActionName, request.FullName, request.Url, request.ParentId, request.Sort, request.Remark, LoginManager.Id, request.Icon, request.MenuType.Value, request.TopMenuKey,request.IsMultiAction);
            return ApiJson();
        }
    }
}