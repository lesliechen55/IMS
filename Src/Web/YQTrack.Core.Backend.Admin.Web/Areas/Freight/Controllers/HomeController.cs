using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Controllers
{
    public class HomeController : BaseFreightController
    {
        private readonly IInquiryService _inquiryService;
        private readonly IMapper _mapper;
        private readonly IHomeService _homeService;

        public HomeController(IInquiryService inquiryService,
                              IMapper mapper,
                              IHomeService homeService)
        {
            _inquiryService = inquiryService;
            _mapper = mapper;
            _homeService = homeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var (totalChannel, totalInquiry, totalQuote, totalCarrier) = await _homeService.GetMainDataAsync();
            return View(new MainResponse
            {
                TotalChannel = totalChannel,
                TotalInquiry = totalInquiry,
                TotalQuote = totalQuote,
                TotalCarrier = totalCarrier
            });
        }

        [HttpGet]
        public IActionResult Channel()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Inquiry()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Inquiry))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetInquiryList(InquiryPageDataRequest request)
        {
            var (outputs, total) = await _inquiryService.GetInquiryList(request.Id, request.Title, request.InquiryNo, (byte?)request.Status, request.Page, request.Limit, request.Publisher, request.PublishStartTime, request.PublishEndTime, request.ExpireStartTime, request.ExpireEndTime);
            var responses = _mapper.Map<IEnumerable<InquiryPageDataResponse>>(outputs);
            var data = new PageResponse<InquiryPageDataResponse>
            {
                Count = total,
                Data = responses
            };

            return Json(data);
        }

        [HttpGet]
        public IActionResult Quote()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Quote))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetQuotePageData(QuotePageDataRequest request)
        {
            var input = _mapper.Map<QuotePageDataInput>(request);
            var (outputs, total) = await _homeService.GetQuotePageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<QuotePageDataResponse>>(outputs);
            var data = new PageResponse<QuotePageDataResponse>
            {
                Count = total,
                Data = responses
            };
            return Json(data);
        }
    }
}