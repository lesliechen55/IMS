using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Input;
using YQTrack.Core.Backend.Admin.CarrierTrack.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Controllers
{
    public class HomeController : BaseCarrierTrackController
    {
        private readonly IHomeService _homeService;
        private readonly IMapper _mapper;

        public HomeController(IHomeService homeService, IMapper mapper)
        {
            _homeService = homeService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetPageData(IndexPageDataRequest request)
        {
            var input = _mapper.Map<IndexPageDataInput>(request);
            var outputs = await _homeService.GetPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<IndexPageDataResponse>>(outputs).ToList();
            return ApiJson(new PageResponse<IndexPageDataResponse>
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
        [PermissionCode(nameof(Add))]
        public async Task<IActionResult> Add(CarrierTrackUserAddRequest request)
        {
            var input = _mapper.Map<CarrierTrackUserAddInput>(request);
            await _homeService.AddAsync(input, LoginManager.Id);
            return ApiJson();
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([NotEmpty, FromQuery]long id, [NotEmpty, FromQuery]long userId)
        {
            var (output, availableTrackNum, buyTotal) = await _homeService.GetByIdAsync(id, userId);
            var response = _mapper.Map<IndexPageDataResponse>(output);
            response.AvailableTrackNum = availableTrackNum;
            response.BuyTotalCount = buyTotal;
            return View(new IframeTransferData<IndexPageDataResponse>
            {
                Id = id.ToString(),
                Data = response
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(CarrierTrackUserEditRequest request)
        {
            await _homeService.EditAsync(request.Id, request.UserId, request.ImportTodayLimit, request.ExportTimeLimit, request.Enable, LoginManager.Id);
            return ApiJson();
        }
    }
}