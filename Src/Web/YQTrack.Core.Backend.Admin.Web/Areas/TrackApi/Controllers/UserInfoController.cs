using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using YQTrack.Core.Backend.Admin.TrackApi.DTO;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Input;
using YQTrack.Core.Backend.Admin.TrackApi.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Controllers
{
    public class UserInfoController : BaseTrackApiController
    {
        private readonly IUserInfoService _userInfoService;
        private readonly ITrackCountService _trackCountService;
        private readonly IUserInvoiceService _userInvoiceService;
        private readonly TrackApiConfig _trackApiConfig;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserInfoController(IUserInfoService userInfoService, ITrackCountService trackCountService, IUserInvoiceService userInvoiceService, IOptions<TrackApiConfig> trackApiConfig, IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _userInfoService = userInfoService;
            _trackCountService = trackCountService;
            _userInvoiceService = userInvoiceService;
            _trackApiConfig = trackApiConfig.Value;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new UserInfoSelectDataResponse());
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        [ResultEncodeFilter]
        public async Task<IActionResult> GetPageData(UserInfoPageDataRequest request)
        {
            var input = _mapper.Map<UserInfoPageDataInput>(request);
            var (outputs, total) = await _userInfoService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<UserInfoPageDataResponse>>(outputs);
            return MyPageJson(data, total);
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add()
        {
            return View(new IframeTransferData
            {
                Id = (await _userInfoService.GetMaxUserNo()).ToString()
            });
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add(UserInfoEditRequest request)
        {
            var input = _mapper.Map<UserInfoEditInput>(request);
            bool success = await _userInfoService.AddAsync(input, LoginManager.Id);
            if (success)
            {
                return ApiJson();
            }
            else
            {
                return ApiJson(new ApiResult<short> { Success = false, Msg = "用户编号已存在，系统已帮你生成新编号，请重新保存", Data = await _userInfoService.GetMaxUserNo() });
            }
        }

        /// <summary>
        /// 重新注册
        /// </summary>
        /// <param name="userNo"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionCode(nameof(Edit))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Reregister([NotEmpty]int userNo)
        {
            return ApiJson(new ApiResult { Success = await _userInfoService.ReregisterAsync(userNo, LoginManager.Id) });
        }

        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> View([Required(AllowEmptyStrings = false), FromQuery]int id)
        {
            UserInfoViewResponse response = _mapper.Map<UserInfoViewResponse>(await _userInfoService.GetViewDataByIdAsync(id));
            return View(new IframeTransferData<UserInfoViewResponse> { Data = response });
        }

        /// <summary>
        /// 查看用户配置详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(View))]
        public async Task<IActionResult> ViewConfig([Required(AllowEmptyStrings = false), FromQuery]int id)
        {
            UserConfigResponse response = _mapper.Map<UserConfigResponse>(await _userInfoService.GetUserConfigByIdAsync(id));
            return View(new IframeTransferData<UserConfigResponse> { Data = response });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([Required(AllowEmptyStrings = false), FromQuery]int id)
        {
            var output = await _userInfoService.GetByIdAsync(id);
            UserInfoResponse response = _mapper.Map<UserInfoResponse>(output);
            return View(new IframeTransferData<UserInfoResponse> { Data = response });
        }

        /// <summary>
        /// 获取消耗记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Edit))]
        public async Task<IActionResult> GetTrackCountListData(TrackCountRequest request)
        {
            var input = _mapper.Map<TrackCountInput>(request);
            var outputs = await _trackCountService.GetListDataAsync(input);

            //DateTime startTime, endTime;
            //for (var date = input.StartTime.Value; date < input.EndTime; date = date.AddDays(1))
            //{
            //    HistoryListOutDto dataDto = list.SingleOrDefault(s => s.FDate == date);
            //    if (dataDto == null)
            //    {
            //        list.Add(new HistoryListOutDto
            //        {
            //            Count = 0,
            //            FDate = date
            //        });
            //    }
            //}
            var data = _mapper.Map<IEnumerable<TrackCountResponse>>(outputs);

            return new JsonResult(new PageResponse<TrackCountResponse>
            {
                Data = data
            });
        }

        /// <summary>
        /// 获取账单记录
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Edit))]
        public async Task<IActionResult> GetUserInvoiceListData(UserInvoiceRequest request)
        {
            var input = _mapper.Map<UserInvoiceInput>(request);
            var outputs = await _userInvoiceService.GetListDataAsync(input);
            var data = _mapper.Map<IEnumerable<UserInvoiceResponse>>(outputs);
            return new JsonResult(new PageResponse<UserInvoiceResponse>
            {
                Data = data
            });
        }

        /// <summary>
        /// 获取账单详情（新页面打开PDF文件）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(View))]
        public async Task<IActionResult> InvoicePdf([Required(AllowEmptyStrings = false), FromQuery]int? id)
        {
            string invoiceId = id.Value.ToString();
            string userNo = invoiceId.Substring(0, invoiceId.Length - 4);
            string path = $"{_trackApiConfig.InvoicePath}/20{invoiceId.Substring(userNo.Length, 2)}/{userNo}/invoice-{invoiceId}.pdf";
            return File(await _httpClientFactory.CreateClient().GetByteArrayAsync(path), "application/pdf");
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(UserInfoEditRequest request)
        {
            var input = _mapper.Map<UserInfoEditInput>(request);
            return ApiJson(new ApiResult { Success = await _userInfoService.EditAsync(input, LoginManager.Id) });
        }

        /// <summary>
        /// 修改用户API状态(启用或者禁用)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> ChangeStatus(ChangeApiStateRequest request)
        {
            var input = _mapper.Map<ChangeApiStateInput>(request);
            await _userInfoService.ChangeStatusAsync(input, LoginManager.Id);
            return ApiJson();
        }
    }
}