using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    /// <summary>
    /// 人工对账
    /// </summary>
    public class ManualReconcileController : BasePayController
    {
        private readonly IMapper _mapper;
        private readonly IManualReconcileService _manualReconcileService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ManualReconcileController(IMapper mapper, IManualReconcileService manualReconcileService,
            IHostingEnvironment hostingEnvironment)
        {
            _mapper = mapper;
            _manualReconcileService = manualReconcileService;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 查看导入对账单记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetPageData(ManualReconcilePageDataRequest request)
        {
            var input = _mapper.Map<ManualReconcilePageDataInput>(request);
            var (outputs, total) = await _manualReconcileService.GetPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<ManualReconcilePageDataResponse>>(outputs);
            return MyPageJson(responses, total);
        }

        /// <summary>
        /// 导入信用卡对账单文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ImportGlocash()
        {
            return View(new IframeTransferData());
        }

        /// <summary>
        /// 执行导入信用卡对账单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> ImportGlocash(ImportGlocashRequest request)
        {
            string jsonData;
            using (var reader = new StreamReader(request.FormFile.OpenReadStream()))
            {
                jsonData = await reader.ReadToEndAsync();
            }
            if (jsonData.IsNullOrWhiteSpace() || request.FormFile.Length <= 0)
            {
                throw new BusinessException($"{request.FormFile.Name}内容为空错误");
            }
            var md5 = FileHelper.GetMD5HashFromFile(request.FormFile.OpenReadStream());
            if (await _manualReconcileService.ExistAsync(md5))
            {
                throw new BusinessException($"检测到该文件:{request.FormFile.FileName}已经导入过了");
            }

            // 存储路径例如: /uploadReconcile/Glocash/202003/
            var uploadRoot = Path.Combine(_hostingEnvironment.ContentRootPath, "uploadReconcile", "Glocash", $"{request.Year.Value.ToString()}{request.Month.Value.ToString().PadLeft(2, '0')}");
            if (!Directory.Exists(uploadRoot))
            {
                Directory.CreateDirectory(uploadRoot);
            }
            var filePath = Path.Combine(uploadRoot, $"{md5}.json");
            await System.IO.File.WriteAllTextAsync(filePath, jsonData);

            await _manualReconcileService.ImportGlocashAsync(jsonData, request.FormFile.FileName, md5, uploadRoot, request.Year.Value, request.Month.Value, request.Remark ?? string.Empty, LoginManager.Id);

            return ApiJson();
        }
    }
}