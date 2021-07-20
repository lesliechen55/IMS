using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    public class PaymentController : BasePayController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IRpcService _rpcService;

        public PaymentController(IPaymentService paymentService, IMapper mapper, IRpcService rpcService)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _rpcService = rpcService;
        }

        [HttpGet]
        public IActionResult Index(PaymentPageDataRequest request)
        {
            return View(new PaymentSelectResponse { Request = request });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetPageData(PaymentPageDataRequest request)
        {
            var input = _mapper.Map<PaymentPageDataInput>(request);
            var (outputs, total) = await _paymentService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<PaymentPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<PaymentPageDataResponse>
            {
                Data = data,
                Count = total
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> View([Required(AllowEmptyStrings = false), FromQuery] long id)
        {
            var response = _mapper.Map<PaymentShowResponse>(await _paymentService.GetByIdAsync(id));
            return View(new IframeTransferData<PaymentShowResponse> { Data = response });
        }

        /// <summary>
        /// 退款
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Refund(RefundRequest request)
        {
            var (provider, status) = await _paymentService.GetRefundByIdAsync(request.OrderId);
            if (provider == PaymentProvider.OfflinePay || provider == PaymentProvider.Unknown || status != PaymentStatus.Success)
            {
                return ApiJson(new ApiResult { Success = false, Msg = "该交易不能退款" });
            }
            var response = await _rpcService.RefundAsync(request.OrderId, request.Reason);
            return ApiJson(new ApiResult { Success = response.Result, Msg = response.Message });
        }
    }
}