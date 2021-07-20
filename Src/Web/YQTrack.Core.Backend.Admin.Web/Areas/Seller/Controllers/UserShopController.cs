using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Seller.DTO.Input;
using YQTrack.Core.Backend.Admin.Seller.Service;
using YQTrack.Core.Backend.Admin.User.DTO;
using YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Response;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Seller.Controllers
{
    public class UserShopController : BaseSellerController
    {
        private readonly IUserShopService _userShopService;
        private readonly IMapper _mapper;

        public UserShopController(IUserShopService userShopService, IMapper mapper)
        {
            _userShopService = userShopService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string userRoute)
        {
            return View(new UserShopSelectDataResponse()
            {
                UserRoute = userRoute,
                UserRouteDto = JsonConvert.DeserializeObject<UserRouteDto>(userRoute)
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        [ResultEncodeFilter]
        public async Task<IActionResult> GetPageData(UserShopPageDataRequest request)
        {
            var input = _mapper.Map<UserShopPageDataInput>(request);
            var (outputs, total) = await _userShopService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<UserShopPageDataResponse>>(outputs);
            return MyPageJson(data, total);
        }

        [HttpGet]
        public IActionResult TrackUploadRecord(TrackUploadRecordRequest request)
        {
            return View(request);
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(TrackUploadRecord))]
        [ResultEncodeFilter]
        public async Task<IActionResult> GetTrackUploadRecord(TrackUploadRecordRequest request)
        {
            var input = _mapper.Map<TrackUploadRecordInput>(request);
            var outputs = await _userShopService.GetTrackUploadRecordAsync(input);
            var data = _mapper.Map<IEnumerable<TrackUploadRecordResponse>>(outputs);
            return MyPageJson(data, 15);
        }
    }
}