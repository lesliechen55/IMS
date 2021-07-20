using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Backend.Message.Model.Models;
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
    public class InvoiceApplyController : BasePayController
    {
        private readonly IInvoiceApplyService _invoiceService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public InvoiceApplyController(IInvoiceApplyService invoiceService, IUserService userService, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(EnumHelper.GetSelectItem<InvoiceAuditStatus>());
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        [ResultEncodeFilter]
        public async Task<IActionResult> GetPageData(InvoiceApplyPageDataRequest request)
        {
            var input = _mapper.Map<InvoiceApplyPageDataInput>(request);
            var (outputs, total) = await _invoiceService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<InvoiceApplyPageDataResponse>>(outputs);
            return MyPageJson(data, total);
        }

        private const string Category = "invoiceInfo";

        [HttpGet]
        [ModelStateValidationFilter]
        [ResultEncodeFilter]
        public async Task<IActionResult> View([NotEmpty, FromQuery]long id)
        {
            var response = _mapper.Map<InvoiceApplyShowResponse>(await _invoiceService.GetByIdAsync(id));
            if (response.TaxPayerCertificateUrl.IsNotNullOrWhiteSpace())
            {
                var urlResult = QiniuHelper.QiniuStorage.GetPrivateImageUrlByUser(response.UserId, Category, response.TaxPayerCertificateUrl, ProcessRules.None, 60);
                if (!urlResult.IsSuccess)
                {
                    throw new BusinessException($"获取图片私有地址失败：{urlResult.Message}");
                }
                response.TaxPayerCertificateUrl = urlResult.ImageUrl;
            }
            return View(new IframeTransferData<InvoiceApplyShowResponse> { Data = response });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult Pass([NotEmpty, FromQuery]long id, [Required(AllowEmptyStrings = false), FromQuery]string invoiceEmail)
        {
            return View(new IframeTransferData<InvoiceApplyPassResponse>
            {
                Data = new InvoiceApplyPassResponse { InvoiceApplyId = id, InvoiceEmail = invoiceEmail }
            });
        }

        /// <summary>
        /// 发票申请审核通过
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Pass(InvoiceApplyPassRequest request)
        {
            var input = _mapper.Map<InvoiceApplyPassInput>(request);
            var userId = await _invoiceService.PassAsync(input, LoginManager.Id);
            var user = await _userService.GetUserBaseInfoAsync(userId);
            if (user.FEmail.IsNotNullOrWhiteSpace())
            {
                var messageModel = new MessageModel()
                {
                    MessageType = MessageTemplateType.InvoiceApplyPass,
                    TemplateData = JsonConvert.SerializeObject(new
                    {
                        request.InvoiceApplyId
                    }),
                    UserId = new UserInfoExt()
                    {
                        FEmail = user.FEmail,
                        FNickname = user.FNickName,
                        FLanguage = user.FLanguage
                    }
                };
                if (!MessageHelper.SendMessage(messageModel))
                {
                    throw new BusinessException("发票申请审核通过邮件发送失败");
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
        /// 发票申请驳回
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Reject(InvoiceApplyRejectRequest request)
        {
            var input = _mapper.Map<InvoiceApplyRejectInput>(request);
            var userId = await _invoiceService.RejectAsync(input, LoginManager.Id);
            var user = await _userService.GetUserBaseInfoAsync(userId);
            if (user.FEmail.IsNotNullOrWhiteSpace())
            {
                var messageModel = new MessageModel()
                {
                    MessageType = MessageTemplateType.InvoiceApplyReject,
                    TemplateData = JsonConvert.SerializeObject(new
                    {
                        request.InvoiceApplyId,
                        request.RejectReason
                    }),
                    UserId = new UserInfoExt()
                    {
                        FEmail = user.FEmail,
                        FNickname = user.FNickName,
                        FLanguage = user.FLanguage
                    }
                };
                if (!MessageHelper.SendMessage(messageModel))
                {
                    throw new BusinessException("发票申请驳回邮件发送失败");
                }
            }
            return ApiJson();
        }
    }
}