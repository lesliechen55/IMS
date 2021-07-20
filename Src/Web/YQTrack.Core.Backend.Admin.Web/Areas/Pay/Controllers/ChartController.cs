using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    /// <summary>
    /// 图表分析控制器
    /// </summary>
    public class ChartController : BasePayController
    {
        private readonly IChartService _chartService;
        private readonly IMapper _mapper;

        public ChartController(IChartService chartService,
            IMapper mapper)
        {
            _chartService = chartService;
            _mapper = mapper;
        }

        #region 订单数量分析

        /// <summary>
        /// 订单数量分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult OrderCountView()
        {
            return View();
        }

        /// <summary>
        /// 拉取订单数量分析统计数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(OrderCountView))]
        public async Task<IActionResult> OrderCountAnalysis(OrderChartRequest request)
        {
            var input = _mapper.Map<OrderChartInput>(request);
            var output = await _chartService.GetOrderCountAnalysisOutputAsync(input);
            var responses = _mapper.Map<ChartResponses>(output);
            return ApiJson(new ApiResult<ChartResponses>(responses));
        }


        #endregion

        #region 订单金额分析

        [HttpGet]
        public IActionResult OrderAmountView()
        {
            return View();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(OrderAmountView))]
        public async Task<IActionResult> OrderAmountAnalysis(OrderChartRequest request)
        {
            var input = _mapper.Map<OrderChartInput>(request);
            var output = await _chartService.GetOrderAmountAnalysisOutputAsync(input);
            var responses = _mapper.Map<ChartResponses>(output);
            return ApiJson(new ApiResult<ChartResponses>(responses));
        }

        #endregion

        #region 交易笔数分析

        /// <summary>
        /// 交易笔数分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PaymentCountView()
        {
            return View();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(PaymentCountView))]
        public async Task<IActionResult> PaymentCountAnalysis(PaymentChartRequest request)
        {
            var input = _mapper.Map<PaymentChartInput>(request);
            var output = await _chartService.GetPaymentCountAnalysisOutputAsync(input);
            var responses = _mapper.Map<ChartResponses>(output);
            return ApiJson(new ApiResult<ChartResponses>(responses));
        }

        #endregion

        #region 交易金额分析

        /// <summary>
        /// 交易笔数分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PaymentAmountView()
        {
            return View();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(PaymentAmountView))]
        public async Task<IActionResult> PaymentAmountAnalysis(PaymentChartRequest request)
        {
            var input = _mapper.Map<PaymentChartInput>(request);
            var output = await _chartService.GetPaymentAmountAnalysisOutputAsync(input);
            var responses = _mapper.Map<ChartResponses>(output);
            return ApiJson(new ApiResult<ChartResponses>(responses));
        }

        #endregion
    }
}