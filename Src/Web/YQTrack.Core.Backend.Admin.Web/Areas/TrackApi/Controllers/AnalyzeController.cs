using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.TrackApi.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Controllers
{
    public class AnalyzeController : BaseTrackApiController
    {
        private readonly IAnalyzeService _analyzeService;
        private readonly IMapper _mapper;

        public AnalyzeController(IAnalyzeService analyzeService, IMapper mapper)
        {
            _analyzeService = analyzeService;
            _mapper = mapper;
        }

        /// <summary>
        /// API用户注册分析图表视图展示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RegisterAnalyzeIndex()
        {
            return View();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(RegisterAnalyzeIndex))]
        public async Task<IActionResult> RegisterAnalyze(ChartRequest request)
        {
            // ReSharper disable once PossibleInvalidOperationException
            var output = await _analyzeService.GetRegisterAnalysisOutputAsync(request.ChartDateType.Value, request.UserNo, request.UserName);
            var responses = _mapper.Map<ChartResponses>(output);
            return MyJson(responses);
        }

        [HttpGet]
        [PermissionCode(nameof(RegisterAnalyzeIndex))]
        public async Task<IActionResult> GetAutoCompleteInfo([FromQuery]string keywords)
        {
            var response = new AutoCompleteResponse();
            if (keywords.IsNullOrWhiteSpace()) return Json(response);
            var outputs = await _analyzeService.GetAutoCompleteOutputAsync(keywords);
            var items = _mapper.Map<IEnumerable<AutoCompleteItemResponse>>(outputs);
            response.Content = items;
            return Json(response);
        }
    }
}