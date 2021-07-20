using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Email.DTO.Input;
using YQTrack.Core.Backend.Admin.Email.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Controllers
{
    /// <summary>
    /// 邮件控制器
    /// </summary>
    public class EmailController : BaseBusinessController
    {
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public EmailController(IMapper mapper, IEmailService emailService)
        {
            _mapper = mapper;
            _emailService = emailService;
        }

        /// <summary>
        /// 主视图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取邮件分页数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionCode(nameof(Index))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetEmailPageData(EmailPageDataRequest request)
        {
            var input = _mapper.Map<EmailRecordPageDataInput>(request);
            var (outputs, total) = await _emailService.GetEmailRecordPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<EmailPageDataResponse>>(outputs);
            return ApiJson(new PageResponse<EmailPageDataResponse>
            {
                Data = responses,
                Count = total
            });
        }

        /// <summary>
        /// 详情视图
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionCode(nameof(Index))]
        [ModelStateValidationFilter]
        public IActionResult Detail([Required(AllowEmptyStrings = false)] string messageId)
        {
            return View(new IframeTransferData { Id = messageId });
        }

        /// <summary>
        /// 查询邮件投递详情
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionCode(nameof(Index))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetDeliveryData([Required(AllowEmptyStrings = false)] string messageId)
        {
            var outputs = await _emailService.GetDeliveryRecordDataOutputAsync(messageId);
            var responses = _mapper.Map<IEnumerable<DeliveryRecordDataResponse>>(outputs).ToList();
            return ApiJson(new PageResponse<DeliveryRecordDataResponse>
            {
                Data = responses,
                Count = responses.Count
            });
        }
    }
}