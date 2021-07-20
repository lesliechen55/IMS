using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.RealTime.DTO.Input;
using YQTrack.Core.Backend.Admin.RealTime.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.RealTime.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.RealTime.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.RealTime.Controller
{
    public class UserPlatformShopController : BaseRealTimeController
    {
        IPlatformShopService _platformService;
        IMapper _mapper;

        public UserPlatformShopController(IPlatformShopService platformService, IMapper mapper)
        {
            _platformService = platformService;
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
        public async Task<IActionResult> GetData(PlatformShopDataRequest request)
        {
            var input = _mapper.Map<PlatformShopDataInput>(request);
            var (outputs, total) = await _platformService.GetUserShopDataAsync(input);

            var data = _mapper.Map<IEnumerable<PlatformShopDataResponse>>(outputs);
            return new JsonResult(new PageResponse<PlatformShopDataResponse>
            {
                Data = data,
                Count = total
            });
        }
    }
}
