using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Deals.DTO.Input;
using YQTrack.Core.Backend.Admin.Deals.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Deals.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Deals.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Deals.Controllers
{
    public class StatisticsServiceController : BaseDealsController
    {
        private readonly IMapper _mapper;
        private readonly IStatisticsService _service;

        public StatisticsServiceController(IMapper mapper, IStatisticsService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetStatisticsListData(StatisticsServiceRequest request)
        {
            var input = _mapper.Map<StatisticsServiceInput>(request);
            var outputs = await _service.GetStatisticsListDataAsync(input);
            var responses = _mapper.Map<StatisticsAllListResponses>(outputs);
            return ApiJson(new ApiResult<StatisticsAllListResponses>(responses));
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetStatisticsContrastData(StatisticsServiceRequest request)
        {

            var input = _mapper.Map<StatisticsServiceInput>(request);
            var outputs = await _service.GetPageDataContrastDataAsync(input);
            var responses = _mapper.Map<IEnumerable<StatisticsAllResponse>>(outputs);
            return ApiJson(new PageResponse<StatisticsAllResponse>
            {
                Data = responses
            });
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetStatisticsMerStartData(StatisticsServiceRequest request)
        {
            var input = _mapper.Map<StatisticsServiceInput>(request);
            var outputs = await _service.GetStatisticsMerStartDataAsync(input);
            var responses = _mapper.Map<IEnumerable<StatisticsMerResponses>>(outputs);
            return ApiJson(new PageResponse<StatisticsMerResponses>
            {
                Data = responses
            });
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetStatisticsMerEndData(StatisticsServiceRequest request)
        {
            var input = _mapper.Map<StatisticsServiceInput>(request);
            var outputs = await _service.GetStatisticsMerEndDataAsync(input);
            var responses = _mapper.Map<IEnumerable<StatisticsMerResponses>>(outputs);
            return ApiJson(new PageResponse<StatisticsMerResponses>
            {
                Data = responses
            });
        }

    }
}