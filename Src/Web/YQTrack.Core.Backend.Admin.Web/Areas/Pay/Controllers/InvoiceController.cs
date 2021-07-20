using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.User.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;
using YQTrack.Core.Backend.Enums.Pay;
using YQTrack.Storage.QiniuOSS;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    public class InvoiceController : BasePayController
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public InvoiceController(IInvoiceService invoiceService, IUserService userService, IMapper mapper)
        {
            _invoiceService = invoiceService;
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
        [ResultEncodeFilter]
        public async Task<IActionResult> GetPageData(InvoicePageDataRequest request)
        {
            var input = _mapper.Map<InvoicePageDataInput>(request);
            var (outputs, total) = await _invoiceService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<InvoicePageDataResponse>>(outputs);
            return MyPageJson(data, total);
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult Add()
        {
            return View(new IframeTransferData<Dictionary<int, string>>
            {
                Data = EnumHelper.GetSelectItem<InvoiceType>()
            });
        }
        private const string category= "invoiceInfo";
        /// <summary>
        /// 上传一般纳税人证明
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionCode(nameof(Add))]
        [ModelStateValidationFilter]
        public IActionResult UploadTaxImage(ImageRequest request)
        {
            string fileName;
            var buff = new byte[request.FormFile.Length];
            using (var stream = request.FormFile.OpenReadStream())
            {
                fileName = $"{Guid.NewGuid().ToString("N")}{Path.GetExtension(request.FormFile.FileName)}";
                
                stream.Read(buff, 0, buff.Length);
            }
            var uploadResult = QiniuHelper.QiniuStorage.UploadFileByUser(request.UserId.Value, category, fileName, buff);
            if (!uploadResult.IsSuccess)
            {
                throw new BusinessException($"图片上传失败：{uploadResult.Message}");
            }
            return MyJson(fileName);
        }

        /// <summary>
        /// 根据邮箱获取用户ID
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [HttpGet]
        [PermissionCode(nameof(Add))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetUserId([Required(AllowEmptyStrings = false), FromQuery]string userEmail)
        {
            var user = _mapper.Map<UserBaseInfoResponse>(await _userService.GetUserBaseInfoByEmailAsync(userEmail));
            if (user == null)
            {
                throw new BusinessException($"{nameof(userEmail)}参数错误,用户不存在");
            }
            return MyJson(user.UserId);
        }

        /// <summary>
        /// 添加发票资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add(InvoiceAddRequest request)
        {
            var user = await _userService.GetUserBaseInfoByEmailAsync(request.UserEmail);
            if (user == null)
            {
                throw new BusinessException($"{nameof(request.UserEmail)}参数错误,用户不存在");
            }
            var input = _mapper.Map<InvoiceAddInput>(request);
            input.FUserId = user.FUserId;
            return ApiJson(new ApiResult { Success = await _invoiceService.AddAsync(input, LoginManager.Id) });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        [ResultEncodeFilter]
        public async Task<IActionResult> View([NotEmpty, FromQuery]long id)
        {
            var response = _mapper.Map<InvoiceShowResponse>(await _invoiceService.GetByIdAsync(id));
            if (response.TaxPayerCertificateUrl.IsNotNullOrWhiteSpace())
            {
                var urlResult = QiniuHelper.QiniuStorage.GetPrivateImageUrlByUser(response.UserId, category, response.TaxPayerCertificateUrl, ProcessRules.None, 60);
                if (!urlResult.IsSuccess)
                {
                    throw new BusinessException($"获取图片私有地址失败：{urlResult.Message}");
                }
                response.ImageSrc = urlResult.ImageUrl;
            }
            return View(new IframeTransferData<InvoiceShowResponse> { Data = response });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([NotEmpty, FromQuery]long id)
        {
            var response = _mapper.Map<InvoiceShowResponse>(await _invoiceService.GetByIdAsync(id));
            if (response.TaxPayerCertificateUrl.IsNotNullOrWhiteSpace())
            {
                var urlResult = QiniuHelper.QiniuStorage.GetPrivateImageUrlByUser(response.UserId, category, response.TaxPayerCertificateUrl, ProcessRules.None, 60);
                if (!urlResult.IsSuccess)
                {
                    throw new BusinessException($"获取图片私有地址失败：{urlResult.Message}");
                }
                response.ImageSrc = urlResult.ImageUrl;
            }
            return View(new IframeTransferData<InvoiceShowResponse> { Data = response });
        }

        /// <summary>
        /// 修改发票资料
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(InvoiceEditRequest request)
        {
            var input = _mapper.Map<InvoiceEditInput>(request);
            await _invoiceService.EditAsync(input, LoginManager.Id);
            return ApiJson();
        }
    }
}