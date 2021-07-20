using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.User.DTO.Input;
using YQTrack.Core.Backend.Admin.User.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Controllers
{
    public class UserUnregisterController : BaseBusinessController
    {
        private readonly IUserUnregisterService _userUnregisterService;
        private readonly IMapper _mapper;

        public UserUnregisterController(IUserUnregisterService userUnregisterService,
                              IMapper mapper)
        {
            _userUnregisterService = userUnregisterService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetPageData(UserPageDataRequest request)
        {
            var input = _mapper.Map<UserPageDataInput>(request);
            var (outputs, total) = await _userUnregisterService.GetPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<UserUnregisterPageDataResponse>>(outputs);
            return ApiJson(new PageResponse<UserUnregisterPageDataResponse>
            {
                Data = responses,
                Count = total
            });
        }
    }
}