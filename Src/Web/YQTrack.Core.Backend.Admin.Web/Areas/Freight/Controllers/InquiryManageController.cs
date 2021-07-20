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

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Controllers
{
    public class InquiryManageController : BaseFreightController
    {
        private readonly IInquiryService _inquiryService;
        private readonly IMapper _mapper;

        public InquiryManageController(IInquiryService inquiryService,
                                       IMapper mapper)
        {
            _inquiryService = inquiryService;
            _mapper = mapper;
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult Index([NotEmpty]long orderId, [Required(AllowEmptyStrings = false)]string invokeElementId)
        {
            return View(new IframeTransferData
            {
                Id = orderId.ToString(),
                InvokeElementId = invokeElementId
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> Reject(InquiryRejectRequest request)
        {
            await _inquiryService.RejectInquiryAsync(LoginManager.Id, request.Id, request.Reason);
            return ApiJson();
        }

        [HttpGet]
        public IActionResult InquiryOrderStatusLog()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(InquiryOrderStatusLog))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetInquiryOrderStatusLogPageData(InquiryOrderStatusLogPageDataRequest request)
        {
            var input = _mapper.Map<InquiryOrderStatusLogPageDataInput>(request);
            var (outputs, total) = await _inquiryService.GetInquiryOrderStatusLogPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<InquiryOrderStatusLogPageDataResponse>>(outputs);
            return ApiJson(new PageResponse<InquiryOrderStatusLogPageDataResponse>
            {
                Count = total,
                Data = responses
            });
        }

    }
}