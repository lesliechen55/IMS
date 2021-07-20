using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class UserController : BaseBusinessController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService,
                              IMapper mapper)
        {
            _userService = userService;
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
        public async Task<IActionResult> GetUserPageData(UserPageDataRequest request)
        {
            if (!string.IsNullOrEmpty(request.Gid))
            {
                request.UserId = YQTrack.Utility.UserIdExtend.GetUserIdByMaskUserId(request.Gid);
            }
            var input = _mapper.Map<UserPageDataInput>(request);
            var (outputs, total) = await _userService.GetUserPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<UserPageDataResponse>>(outputs);
            return ApiJson(new PageResponse<UserPageDataResponse>
            {
                Data = responses,
                Count = total
            });
        }

        [HttpGet]
        public IActionResult Feedback()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Feedback))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetUserFeedbackPageData(UserFeedbackPageDataRequest request)
        {
            var input = _mapper.Map<UserFeedbackPageDataInput>(request);
            var (outputs, total) = await _userService.GetUserFeedbackPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<UserFeedbackPageDataResponse>>(outputs);
            return ApiJson(new PageResponse<UserFeedbackPageDataResponse>
            {
                Data = responses,
                Count = total
            });
        }

        [HttpGet]
        public IActionResult Reply([NotEmpty, FromQuery]string feedBackIds)
        {
            ViewBag.Ids = feedBackIds;
            return View();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Reply([NotEmpty]string feedBackIds, [Required(AllowEmptyStrings = false)]string content)
        {
            await _userService.Reply(feedBackIds, content, LoginManager.Id);
            return ApiJson();
        }

        [HttpGet]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> Detail([NotEmpty]long userId)
        {
            var output = await _userService.GetDetailAsync(userId);
            var response = _mapper.Map<UserDetailResponse>(output);
            return View(response);
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> DeleteUser([NotEmpty]long userId, [Required(AllowEmptyStrings = false)]string email)
        {
            var (success, message) = await _userService.DeleteUserAsync(userId, email);
            ApiResult ret = new ApiResult()
            {
                Success = success,
                Msg = message
            };
            return ApiJson(ret);
        }
    }
}