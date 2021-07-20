using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Backend.Message.Model.Models;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.Core.Message;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.User.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;
using YQTrack.Core.Backend.Enums.Pay;
using YQTrack.Storage.QiniuOSS;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    public class OfflinePaymentController : BasePayController
    {
        private readonly IOfflinePaymentService _offlineService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public OfflinePaymentController(IOfflinePaymentService invoiceService, IUserService userService, IMapper mapper)
        {
            _offlineService = invoiceService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(EnumHelper.GetSelectItem<OfflineAuditStatus>());
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        [ResultEncodeFilter]
        public async Task<IActionResult> GetPageData(OfflinePaymentPageDataRequest request)
        {
            var input = _mapper.Map<OfflinePaymentPageDataInput>(request);
            var (outputs, total) = await _offlineService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<OfflinePaymentPageDataResponse>>(outputs);
            return MyPageJson(data, total);
        }

        private const string Category = "offlinePay";

        [HttpGet]
        [ModelStateValidationFilter]
        [ResultEncodeFilter]
        public async Task<IActionResult> View([NotEmpty, FromQuery]long id)
        {
            var response = _mapper.Map<OfflinePaymentShowResponse>(await _offlineService.GetByIdAsync(id));
            if (response.TransferPhotoUrl.IsNotNullOrWhiteSpace())
            {
                var urlResult = QiniuHelper.QiniuStorage.GetPrivateImageUrlByUser(response.UserId, Category, response.TransferPhotoUrl, ProcessRules.None, 60);
                if (!urlResult.IsSuccess)
                {
                    throw new BusinessException($"获取图片私有地址失败：{urlResult.Message}");
                }
                response.TransferPhotoUrl = urlResult.ImageUrl;
            }
            return View(new IframeTransferData<OfflinePaymentShowResponse> { Data = response });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult Pass([NotEmpty, FromQuery]long id)
        {
            return View(new IframeTransferData
            {
                Id = id.ToString()
            });
        }

        /// <summary>
        /// 线下交易审核通过
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Pass(OfflinePaymentPassRequest request)
        {
            var input = _mapper.Map<OfflinePaymentPassInput>(request);
            var userId = await _offlineService.PassAsync(input, LoginManager.Id);
            var user = await _userService.GetUserBaseInfoAsync(userId);
            if (user.FEmail.IsNotNullOrWhiteSpace())
            {
                var messageModel = new MessageModel
                {
                    MessageType = MessageTemplateType.OfflinePaymentPass,
                    TemplateData = JsonConvert.SerializeObject(new
                    {
                        request.OfflinePaymentId
                    }),
                    UserId = new UserInfoExt
                    {
                        FEmail = user.FEmail,
                        FNickname = user.FNickName,
                        FLanguage = user.FLanguage
                    }
                };
                if (!MessageHelper.SendMessage(messageModel))
                {
                    throw new BusinessException("线下交易审核通过邮件发送失败");
                }
            }
            return ApiJson();
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult Reject([NotEmpty, FromQuery]long id)
        {
            return View(new IframeTransferData { Id = id.ToString() });
        }

        /// <summary>
        /// 线下交易驳回
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Reject(OfflinePaymentRejectRequest request)
        {
            var input = _mapper.Map<OfflinePaymentRejectInput>(request);
            var userId = await _offlineService.RejectAsync(input, LoginManager.Id);
            var user = await _userService.GetUserBaseInfoAsync(userId);
            if (user.FEmail.IsNotNullOrWhiteSpace())
            {
                var messageModel = new MessageModel
                {
                    MessageType = MessageTemplateType.OfflinePaymentReject,
                    TemplateData = JsonConvert.SerializeObject(new
                    {
                        request.OfflinePaymentId,
                        request.RejectReason
                    }),
                    UserId = new UserInfoExt
                    {
                        FEmail = user.FEmail,
                        FNickname = user.FNickName,
                        FLanguage = user.FLanguage
                    }
                };
                if (!MessageHelper.SendMessage(messageModel))
                {
                    throw new BusinessException("线下交易驳回邮件发送失败");
                }
            }
            return ApiJson();
        }
    }
}