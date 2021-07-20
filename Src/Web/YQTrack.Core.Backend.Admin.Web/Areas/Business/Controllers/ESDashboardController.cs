using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.DTO;
using YQTrack.Core.Backend.Admin.DTO.Output;
using YQTrack.Core.Backend.Admin.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Controllers
{
    public class ESDashboardController : BaseBusinessController
    {
        private readonly IMapper _mapper;
        private readonly IESDashboardService _eSDashboardService;
        private readonly KibanaConfig _kibanaConfig;

        public ESDashboardController(IMapper mapper, IESDashboardService eSDashboardService, IOptions<KibanaConfig> kibanaConfig)
        {
            _mapper = mapper;
            _eSDashboardService = eSDashboardService;
            _kibanaConfig = kibanaConfig.Value;
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Set([NotEmpty, FromQuery]int? id)
        {
            var output = await _eSDashboardService.GetByIdAsync(id.Value);
            var response = _mapper.Map<ESDashboardDetailResponse>(output);
            return View(new IframeTransferData<ESDashboardDetailResponse>
            {
                Id = id.ToString(),
                Data = response
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Set(ESDashboardEditRequest request)
        {
            var input = _mapper.Map<ESDashboardDto>(request);
            await _eSDashboardService.SetAsync(input);
            return ApiJson();
        }

        [HttpGet]
        [PermissionCode(nameof(Set))]
        public async Task<IActionResult> GetDataByCategory([NotEmpty]string category)
        {
            var outputs = await _eSDashboardService.GetDataByCategoryAsync(category);
            var responses = _mapper.Map<List<ESFieldResponse>>(outputs);
            return new JsonResult(new PageResponse<ESFieldResponse>
            {
                Data = responses,
                Count = responses.Count
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Index([NotEmpty, FromQuery]string permissionCode)
        {
            var output = await _eSDashboardService.GetByPermissionCodeAsync(permissionCode);
            var response = _mapper.Map<ESDashboardDetailResponse>(output);
            //Kibana访问账号(模块优先级高于全局)
            response.ESDashboard.Username = response.ESDashboard.Username.IsNullOrWhiteSpace() ? _kibanaConfig.Username : response.ESDashboard.Username;
            response.ESDashboard.Password = response.ESDashboard.Password.IsNullOrWhiteSpace() ? _kibanaConfig.Password : response.ESDashboard.Password;
            return View(new IframeTransferData<ESDashboardDetailResponse>
            {
                Id = permissionCode,
                Data = response
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> View([NotEmpty, FromQuery]string userPass, [NotEmpty, FromQuery]string src)
        {
            WebClient webClient = new WebClient();
            string auth = $"Basic {Convert.ToBase64String(Encoding.UTF8.GetBytes(userPass))}";
            webClient.Headers["Authorization"] = auth;
            string pageHtml = await webClient.DownloadStringTaskAsync(src);//从指定网站下载数据
            return MyJson(pageHtml);
        }
    }
}