using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.WebCore;
using YQTrack.SRVI.Payment.Model;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    public class TransactionController : BasePayController
    {
        private readonly IRpcService _rpcService;

        public TransactionController(IRpcService rpcService)
        {
            _rpcService = rpcService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> Query(TransactionQueryRequest request)
        {
            var response = await _rpcService.TransactionQueryAsync((SRVI.Payment.Enum.PaymentProvider)request.PaymentProvider, request.OrderId ?? 0, request.TradeNo, new RequestContext(LoginManager.Id, HttpContext.Connection.RemoteIpAddress.ToString(), false));
            return ApiJson(new ApiResult<PaymentResponse>(response));
        }
    }
}