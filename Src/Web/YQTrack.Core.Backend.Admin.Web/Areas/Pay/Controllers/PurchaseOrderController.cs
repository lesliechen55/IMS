using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    public class PurchaseOrderController : BasePayController
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IMapper _mapper;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService, IMapper mapper)
        {
            _purchaseOrderService = purchaseOrderService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index([FromQuery]PurchaseOrderPageDataRequest request)
        {
            return View(new PurchaseOrderSelectResponse { Request = request });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetPageData(PurchaseOrderPageDataRequest request)
        {
            var input = _mapper.Map<PurchaseOrderPageDataInput>(request);
            var (outputs, total) = await _purchaseOrderService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<PurchaseOrderPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<PurchaseOrderPageDataResponse>
            {
                Data = data,
                Count = total
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> ViewProduct([Required(AllowEmptyStrings = false), FromQuery]long id)
        {
            var output = await _purchaseOrderService.GetByIdAsync(id);
            var response = _mapper.Map<PurchaseOrderShowResponse>(output);
            if (output.IsShowPresentPage)
            {
                var (dictionary, email) = await _purchaseOrderService.GetSkuAsync(output.FUserId, output.FCurrencyType);
                response.SkuDic = dictionary;
                response.UserEmail = email;
            }
            return View(new IframeTransferData<PurchaseOrderShowResponse> { Data = response, Id = response.PurchaseOrderId.ToString() });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Present(PurchaseOrderPresentRequest request)
        {
            var orderNo = await _purchaseOrderService.PresentAsync(request.OrderId, request.SkuId, request.Quantity, LoginManager.Id);
            return MyJson(orderNo.ToString());
        }
    }
}