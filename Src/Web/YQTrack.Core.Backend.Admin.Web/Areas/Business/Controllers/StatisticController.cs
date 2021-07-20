using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Log.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response;
using YQTrack.Core.Backend.Admin.WebCore;
using YQTrack.Core.Backend.Enums.Pay;
using YQTrack.Core.Backend.Enums.User;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Controllers
{
    public class StatisticController : BaseBusinessController
    {
        private readonly IMapper _mapper;
        private readonly IAnalysisService _analysisService;
        private const string Controller = "_Statistic_";

        public StatisticController(IMapper mapper,
            IAnalysisService analysisService)
        {
            _mapper = mapper;
            _analysisService = analysisService;
        }

        [HttpGet]
        public IActionResult AnalysisView()
        {
            return View();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(AnalysisView))]
        public async Task<IActionResult> GetAnalysisData([Required]AnalysisType? analysisType, [Required] ChartDateType? chartDateType)
        {
            var output = await _analysisService.GetAnalysisDataAsync(analysisType.Value, chartDateType.Value);
            var responses = _mapper.Map<ChartResponse>(output);
            return MyJson(responses);
        }
    }
}