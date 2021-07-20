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
    public class ExportController : BaseFreightController
    {
        private readonly IExportService _exportService;
        private readonly IMapper _mapper;
        private readonly IHomeService _homeService;

        public ExportController(IExportService exportService,
                                IMapper mapper,
                                IHomeService homeService)
        {
            _exportService = exportService;
            _mapper = mapper;
            _homeService = homeService;
        }

        [HttpGet]
        [FileDownload]
        public async Task<IActionResult> ValidChannelInfo()
        {
            var outputs = await _exportService.GetChannelInfoAsync();
            var responses = _mapper.Map<IEnumerable<ExportValidChannelResponse>>(outputs);
            var bytes = ExcelHelper.GenerateExcel(responses);
            return FileExcel(bytes);
        }


        [HttpGet]
        [FileDownload]
        public async Task<IActionResult> InvalidChannelInfo()
        {
            var outputs = await _exportService.GetInvalidChannelInfoAsync();
            var responses = _mapper.Map<IEnumerable<ExportInvalidChannelResponse>>(outputs);
            var bytes = ExcelHelper.GenerateExcel(responses);
            return FileExcel(bytes);
        }

        [HttpGet]
        [FileDownload]
        public async Task<IActionResult> CarrierInfo()
        {
            var outputs = await _exportService.GetCarrierInfoAsync();
            var responses = _mapper.Map<IEnumerable<ExportCarrierResponse>>(outputs);
            var bytes = ExcelHelper.GenerateExcel(responses);
            return FileExcel(bytes);
        }

        [HttpGet]
        [FileDownload]
        [ModelStateValidationFilter]
        public async Task<IActionResult> InquiryInfo([FromQuery]InquiryPageDataRequest request)
        {
            var outputs = await _exportService.GetInquiryInfoAsync(request.Id, request.Title, request.InquiryNo, (byte?)request.Status, request.Publisher, request.PublishStartTime, request.PublishEndTime, request.ExpireStartTime, request.ExpireEndTime);
            var responses = _mapper.Map<IEnumerable<InquiryPageDataResponse>>(outputs);
            var bytes = ExcelHelper.GenerateExcel(responses);
            return FileExcel(bytes);
        }

        [HttpGet]
        [FileDownload]
        [ModelStateValidationFilter]
        public async Task<IActionResult> QuoteInfo([FromQuery] QuotePageDataRequest request)
        {
            var input = _mapper.Map<QuotePageDataInput>(request);
            var outputs = await _homeService.GetQuotePageInfoAsync(input);
            var responses = _mapper.Map<IEnumerable<QuotePageDataResponse>>(outputs);
            var bytes = ExcelHelper.GenerateExcel(responses);
            return FileExcel(bytes);
        }

    }
}