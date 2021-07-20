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
    public class ChannelController : BaseFreightController
    {
        private readonly IHomeService _homeService;
        private readonly IMapper _mapper;

        public ChannelController(IHomeService homeService,
                                 IMapper mapper)
        {
            _homeService = homeService;
            _mapper = mapper;
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetChannelPageData(ChannelPageDataRequest request)
        {
            var input = _mapper.Map<ChannelPageDataInput>(request);
            var (outputs, total) = await _homeService.GetChannelPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<ChannelPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<ChannelPageDataResponse>
            {
                Data = responses,
                Count = total
            });
        }

        [HttpGet]
        [FileDownload]
        [ModelStateValidationFilter]
        public async Task<IActionResult> ExportChannelExcel([FromQuery]ChannelPageDataRequest request)
        {
            var input = _mapper.Map<ChannelPageDataInput>(request);
            var outputs = await _homeService.GetChannelExcelAsync(input);
            var responses = _mapper.Map<IEnumerable<ChannelPageDataResponse>>(outputs);
            var bytes = ExcelHelper.GenerateExcel(responses);
            return FileExcel(bytes);
        }
    }
}