using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    public class ActivityController : BasePayController
    {
        IActivityService _activityService;
        IProductService _productService;
        IProductSkuService _productSkuService;
        IActivityCouponService _activityCouponService;
        IMapper _mapper;

        public ActivityController(
            IActivityService activityService, 
            IProductService productService, 
            IProductSkuService productSkuService,
            IActivityCouponService activityCouponService,
            IMapper mapper)
        {
            _activityService = activityService;
            _productService = productService;
            _productSkuService = productSkuService;
            _activityCouponService = activityCouponService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productSkuService.GetAllProductAsync();
            return View(products);
            //return await Task.FromResult(View());
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetPageData(ActivityPageDataRequest request)
        {
            var input = _mapper.Map<ActivityPageDataInput>(request);
            var (outputs, total) = await _activityService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<ActivityPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<ActivityPageDataResponse>
            {
                Data = data,
                Count = total
            });
        }

        [PermissionCode(nameof(Add))]
        public async Task<IActionResult> GetProducts()
        {
            var data = await _productSkuService.GetAllProductAsync();
            return ApiJson(new ApiResult<Dictionary<long, string>>(data));
        }

        [PermissionCode(nameof(Add))]
        public async Task<IActionResult> GetSkus(long productId)
        {
            var data = await _productSkuService.GetSkuListByProductIdAsync(productId);
            return ApiJson(new ApiResult<Dictionary<string, string>>(data));
        }

        [HttpGet]
        //[ModelStateValidationFilter]
        public async Task<IActionResult> Add()
        {
            return await Task.FromResult(View(new IframeTransferData{Id = ""}));
        }

        [HttpGet]
        [PermissionCode(nameof(Add))]
        public async Task<IActionResult> AddRule()
        {
            return await Task.FromResult(View());
        }

        [HttpGet]
        //[ModelStateValidationFilter]
        [PermissionCode(nameof(Add))]
        public async Task<IActionResult> AddCoupon([Required(AllowEmptyStrings = false), FromQuery] int id)
        {
            var output = await _activityService.GetByIdAsync(id);
            var response = _mapper.Map<ActivityPageDataResponse>(output);
            return View(new IframeTransferData<ActivityPageDataResponse>
            {
                Id = id.ToString(),
                Data = response
            });
        }

        /// <summary>
        /// 发放优惠券
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        //[ModelStateValidationFilter]
        public async Task<IActionResult> AddCoupon(ActivityCouponEditRequest request)
        {
            if (!request.Rules.Any())
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置活动规则" });
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请输入用户Email" });
            }

            var input = _mapper.Map<ActivityCouponEditInput>(request);

            input.Rule = JsonConvert.SerializeObject(request.Rules);

            var result = await _activityCouponService.AddActivityCouponAsync(input, LoginManager.Id);
            if (result>0)
            {
                return ApiJson(new ApiResult<short> { Success = true, Msg = $"成功发放优惠券{result}条。" });
            }
            else
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "可能是数据已存在" });
            }
        }


        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add(ActivityEditRequest request)
        {
            //进行校验
            //通用活动
            if (request.ActivityType == ActivityType.None)
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置活动类型" });
            }
            if (request.DiscountType == ActivityDiscountType.None)
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置优惠类型" });
            }
            if (request.CouponMode == CouponMode.None)
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置优惠方式" });
            }
            if (request.ProductId == 0)
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置参与活动的商品" });
            }
            if (request.StartTime > request.EndTime)
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置有有效的活动开始和结束日期" });
            }
            if (!request.Rules.Any())
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置活动规则" });
            }
            if (request.Term > 0 && 
                (!request.Days.HasValue || request.Days.Value <= 0))
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置有效期天数" });
            }
            
            if (request.ActivityType == ActivityType.General)
            {
                if (request.DiscountType != ActivityDiscountType.AmountDiscount)
                {
                    return ApiJson(new ApiResult<short> { Success = false, Msg = "通用活动只能使用金额优惠" });
                }
                request.BusinessType = BusinessCtrlType.None;
            }
            if (request.ActivityType == ActivityType.Coupon)
            {
                if (request.DiscountType == ActivityDiscountType.QuotaDiscount)
                {
                    if (request.BusinessType == BusinessCtrlType.None)
                    {
                        return ApiJson(new ApiResult<short> { Success = false, Msg = "设置了额赠送优惠需要设置额度的业务类型" });
                    }
                    if (request.Rules.Any(x => x.c != Convert.ToByte(CurrencyType.None)))
                    {
                        return ApiJson(new ApiResult<short> { Success = false, Msg = "设置了额度赠送优惠,则活动规则不能指定货币" });
                    }
                }
                else
                {
                    request.BusinessType = BusinessCtrlType.None;
                }
            }
            //金额优惠规则必须指定货币
            if (request.DiscountType == ActivityDiscountType.AmountDiscount &&
                request.Rules.Any(x => x.c == Convert.ToByte(CurrencyType.None)))
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "设置了金额优惠,则活动规则必须指定货币" });
            }
            //重置错误/无效的参数//更新或者入库
            int idx = 0;
            request.Rules = request.Rules.OrderBy(x => x.c).OrderBy(x => x.t).ToArray();
            foreach (var item in request.Rules)
            {
                idx++;
                item.id = idx;
            }
            
            var input = _mapper.Map<ActivityEditInput>(request);
            input.SkuCodes = JsonConvert.SerializeObject(request.SkuCodes);
            input.Rules = JsonConvert.SerializeObject(request.Rules);
            input.Status = ActivityStatus.None;//ActivityStatus.Awaiting
            input.Term = request.Term > 0 ? request.Days.Value : request.Term;
            var result = await _activityService.AddActivityAsync(input, LoginManager.Id);

            if (result)
            {
                return ApiJson();
            }
            else
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "保存活动出现异常！" });
            }
        }


        [HttpGet]
        public async Task<IActionResult> View([Required(AllowEmptyStrings = false), FromQuery] int id) { 
            var output = await _activityService.GetByIdAsync(id);
            var response = _mapper.Map<ActivityPageDataResponse>(output);
            return View(new IframeTransferData<ActivityPageDataResponse>
            {
                Id = id.ToString(),
                Data = response
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([Required(AllowEmptyStrings = false), FromQuery] int id)
        {
            var output = await _activityService.GetByIdAsync(id);
            var response = _mapper.Map<ActivityPageDataResponse>(output);
            return View(new IframeTransferData<ActivityPageDataResponse>
            {
                Id = id.ToString(),
                Data = response
            });
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(ActivityEditRequest request)
        {
            #region 数据校验
            if (request.ActivityType == ActivityType.None)
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置活动类型" });
            }
            if (request.DiscountType == ActivityDiscountType.None)
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置优惠类型" });
            }
            if (request.CouponMode == CouponMode.None)
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置优惠方式" });
            }
            if (request.ProductId == 0)
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置参与活动的商品" });
            }
            if (request.StartTime > request.EndTime)
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置有有效的活动开始和结束日期" });
            }
            if (!request.Rules.Any())
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置活动规则" });
            }
            if (request.Term > 0 &&
                (!request.Days.HasValue || request.Days.Value <= 0))
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "请设置有效期天数" });
            }
            //活动类型为平台通用
            if (request.ActivityType == ActivityType.General)
            {
                if (request.DiscountType != ActivityDiscountType.AmountDiscount)
                {
                    return ApiJson(new ApiResult<short> { Success = false, Msg = "通用活动只能使用金额优惠" });
                }
                request.BusinessType = BusinessCtrlType.None;
            }

            //活动类型为优惠券
            if (request.ActivityType == ActivityType.Coupon)
            {
                //优惠方式为额度优惠
                if (request.DiscountType == ActivityDiscountType.QuotaDiscount)
                {
                    if (request.BusinessType == BusinessCtrlType.None)
                    {
                        return ApiJson(new ApiResult<short> { Success = false, Msg = "设置了额赠送优惠需要设置额度的业务类型" });
                    }
                    if (request.Rules.Any(x => x.c != Convert.ToByte(CurrencyType.None)))
                    {
                        return ApiJson(new ApiResult<short> { Success = false, Msg = "设置了额度赠送优惠,则活动规则不能指定货币" });
                    }
                }
                else
                {
                    //金额优惠 不需要设置业务类型
                    request.BusinessType = BusinessCtrlType.None;
                }
            }
            //金额优惠规则必须指定货币
            if (request.DiscountType == ActivityDiscountType.AmountDiscount &&
                request.Rules.Any(x => x.c == Convert.ToByte(CurrencyType.None)))
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "设置了金额优惠,则活动规则必须指定货币" });
            }
            #endregion


            request.Rules = request.Rules.OrderBy(x => x.c).OrderBy(x => x.t).ToArray();

            var input = _mapper.Map<ActivityEditInput>(request);
            input.SkuCodes = JsonConvert.SerializeObject(request.SkuCodes);
            input.Rules = JsonConvert.SerializeObject(request.Rules);
            
            input.Term = request.Term > 0 ? request.Days.Value : request.Term;

            var result = await _activityService.EditAsync(input, LoginManager.Id);
            if (result)
            {
                return ApiJson();
            }
            else
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "更新活动出现异常！" });
            }
        }

        /// <summary>
        /// 修改活动状态(启用或者禁用)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> ChangeStatus(ActivityEditStatusInput request)
        {
            await _activityService.ChangeStatusAsync(request, LoginManager.Id);
            return ApiJson();
        }
    }
}