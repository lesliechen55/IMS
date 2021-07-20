using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.Core.Enums;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Controllers
{
    public class SendTaskController : BaseMessageController
    {
        private readonly IMapper _mapper;
        private readonly ISendTaskService _service;
        private readonly IProjectService _projectService;

        public SendTaskController(IMapper mapper, ISendTaskService service, IProjectService projectService)
        {
            _mapper = mapper;
            _service = service;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            TemplateTypeSelectResponse res = new TemplateTypeSelectResponse();
            res.Projects = _mapper.Map<List<ProjectResponse>>(await _projectService.GetAllDataAsync());

            res.Channels = _projectService.GetAllChannels();
            return View(res);
        }

        /// <summary>
        /// 获取发送任务分页列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionCode(nameof(Index))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetPageData(SendTaskPageDataRequest request)
        {
            var input = _mapper.Map<SendTaskPageDataInput>(request);
            var (outputs, total) = await _service.GetPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<SendTaskPageDataResponse>>(outputs);
            return ApiJson(new PageResponse<SendTaskPageDataResponse>
            {
                Count = total,
                Data = responses
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult Add([Required(AllowEmptyStrings = false), FromQuery]string templateName, [NotEmpty, FromQuery]ChannelSend channel, [NotEmpty, FromQuery]long id)
        {
            UserRoleTypeResponse res = new UserRoleTypeResponse()
            {
                Channel = channel,
                TemplateName = templateName,
                UserRoleTypes = GetUserRoleTypeSelect()
            };
            return View(new IframeTransferData<UserRoleTypeResponse>
            {
                Data = res,
                Id = id.ToString()
            });
        }

        /// <summary>
        /// 获取全部角色类型
        /// </summary>
        /// <returns></returns>
        private static List<UserRoleType> GetUserRoleTypeSelect()
        {
            var res = new List<UserRoleType>();
            foreach (UserRoleTypeEnum item in Enum.GetValues(typeof(UserRoleTypeEnum)))
            {
                res.Add(new UserRoleType()
                {
                    UserRoleTypeId = (int)item,
                    UserRoleTypeName = item.GetDescription()
                });
            }

            return res;
        }

        /// <summary>
        /// 添加发送任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add(SendTaskEditRequest request)
        {
            var input = _mapper.Map<SendTaskEditInput>(request);
            return ApiJson(new ApiResult { Success = await _service.AddAsync(input, LoginManager.NickName) });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([NotEmpty, FromQuery]long id)
        {
            var output = await _service.GetByIdAsync(id);
            SendTaskEditResponse res = _mapper.Map<SendTaskEditResponse>(output);
            res.UserRoleTypes = GetUserRoleTypeSelect();
            return View(new IframeTransferData<SendTaskEditResponse>
            {
                Data = res,
                Id = id.ToString()
            });
        }
        /// <summary>
        /// 修改发送任务
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(SendTaskEditRequest request)
        {
            var input = _mapper.Map<SendTaskEditInput>(request);
            return ApiJson(new ApiResult { Success = await _service.EditAsync(input, LoginManager.NickName) });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult SendTemplateTest([Required(AllowEmptyStrings = false), FromQuery]string templateName, [NotEmpty, FromQuery]ChannelSend channel, [NotEmpty, FromQuery]long id)
        {
            UserRoleTypeResponse res = new UserRoleTypeResponse()
            {
                Channel = channel,
                TemplateName = templateName
            };
            return View(new IframeTransferData<UserRoleTypeResponse>
            {
                Data = res,
                Id = id.ToString()
            });
        }

        /// <summary>
        /// 测试发送语言模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> SendTemplateTest(SendTemplateTestRequest request)
        {
            var input = _mapper.Map<SendTemplateTestInput>(request);
            return ApiJson(new ApiResult { Success = await _service.SendTemplateTestAsync(input) });
        }
    }
}