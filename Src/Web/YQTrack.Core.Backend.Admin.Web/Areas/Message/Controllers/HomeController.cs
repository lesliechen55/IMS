using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Message.Core.Enums;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;
using YQTrack.Core.Backend.Admin.Message.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Controllers
{
    public class HomeController : BaseMessageController
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _service;

        public HomeController(IMapper mapper, IProjectService service)
        {
            _mapper = mapper;
            _service = service;
        }

        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [PermissionCode("Project")]
        public IActionResult GetProjectData()
        {
            var outputs = _service.GetAllData();
            return ApiJson(new ApiResult<List<ProjectOutput>>(outputs));
        }

        /// <summary>
        /// 获取发送渠道数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [PermissionCode("Channel")]
        public IActionResult GetChannelData()
        {
            List<ChannelResponse> result = new List<ChannelResponse>();
            foreach (ChannelSend item in Enum.GetValues(typeof(ChannelSend)))
            {
                result.Add(new ChannelResponse()
                {
                    ChannelId = (int)item,
                    ChannelName = item.ToString()
                });
            }
            return ApiJson(new ApiResult<List<ChannelResponse>>(result));
        }
    }
}