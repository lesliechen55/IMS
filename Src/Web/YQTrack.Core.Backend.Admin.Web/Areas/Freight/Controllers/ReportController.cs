using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Controllers
{
    public class ReportController : BaseFreightController
    {
        private readonly IMapper _mapper;
        private readonly IReportService _reportService;

        public ReportController(IMapper mapper, IReportService reportService)
        {
            _mapper = mapper;
            _reportService = reportService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetReportPageData(ReportPageDataRequest request)
        {
            var input = _mapper.Map<ReportPageDataInput>(request);
            var (outputs, total) = await _reportService.GetReportPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<ReportPageDataResponse>>(outputs);
            return ApiJson(new PageResponse<ReportPageDataResponse>
            {
                Data = responses,
                Count = total
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Process([NotEmpty, FromQuery]long id)
        {
            var (status, desc) = await _reportService.GetStatusAsync(id);
            return View(new IframeTransferData { Id = id.ToString(), InvokeElementId = $"{status.ToString()}|{desc ?? string.Empty}" });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Process([NotEmpty]long id, [Required]ProcessReportStatusEnum? status, [Required(AllowEmptyStrings = false), MaxLength(200)]string remark, [MaxLength(200)]string detail)
        {
            // ReSharper disable once PossibleInvalidOperationException
            if (status.Value == ProcessReportStatusEnum.ValidReport && string.IsNullOrWhiteSpace(detail))
            {
                return ApiJson(new ApiResult { Success = false, Msg = $"当处理为有效举报,参数:{nameof(detail)}不能为空" });
            }
            await _reportService.ProcessAsync(id, status.Value, remark, detail);
            return ApiJson();
        }
    }
}