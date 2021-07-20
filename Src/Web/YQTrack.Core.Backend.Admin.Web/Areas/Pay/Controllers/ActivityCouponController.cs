using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    public class ActivityCouponController : BasePayController
    {
        IActivityCouponService _activityCouponService;
        IMapper _mapper;

        public ActivityCouponController(
            IActivityCouponService activityCouponService,
            IMapper mapper)
        {
            _activityCouponService = activityCouponService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return await Task.FromResult(View());
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetPageData(ActivityCouponPageDataRequest request)
        {
            var input = _mapper.Map<ActivityCouponPageDataInput>(request);
            var (outputs, total) = await _activityCouponService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<ActivityCouponPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<ActivityCouponPageDataResponse>
            {
                Data = data,
                Count = total
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add()
        {
            return await Task.FromResult(View(new IframeTransferData { Id = "" }));
        }

    }
}
