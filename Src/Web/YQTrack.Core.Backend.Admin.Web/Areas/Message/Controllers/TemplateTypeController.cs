using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;
using YQTrack.Core.Backend.Admin.Message.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Controllers
{
    public class TemplateTypeController : BaseMessageController
    {
        private readonly IMapper _mapper;
        private readonly ITemplateTypeService _service;
        private readonly IProjectService _projectService;

        public TemplateTypeController(IMapper mapper, ITemplateTypeService service, IProjectService projectService)
        {
            _mapper = mapper;
            _service = service;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await GetPageSelectData());
        }

        /// <summary>
        /// 获取下拉数据
        /// </summary>
        /// <returns></returns>
        private async Task<TemplateTypeSelectResponse> GetPageSelectData()
        {
            TemplateTypeSelectResponse res = new TemplateTypeSelectResponse();
            res.Projects = _mapper.Map<List<ProjectResponse>>(await _projectService.GetAllDataAsync());

            res.Channels = _projectService.GetAllChannels();
            return res;
        }

        /// <summary>
        /// 获取基础模板列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionCode(nameof(Index))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetPageData(TemplateTypePageDataRequest request)
        {
            var input = _mapper.Map<TemplateTypePageDataInput>(request);
            var (outputs, total) = await _service.GetPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<TemplateTypePageDataResponse>>(outputs);
            return ApiJson(new PageResponse<TemplateTypePageDataResponse>
            {
                Count = total,
                Data = responses
            });
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View(new IframeTransferData<TemplateTypeSelectResponse>
            {
                Data = await GetPageSelectData()
            });
        }
        /// <summary>
        /// 添加基础模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add(TemplateTypeEditRequest request)
        {
            var input = _mapper.Map<TemplateTypeEditInput>(request);
            return ApiJson(new ApiResult { Success = await _service.AddAsync(input) });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([NotEmpty, FromQuery]long id)
        {
            var output = await _service.GetByIdAsync(id);
            var response = _mapper.Map<TemplateTypeEditResponse>(output);
            response.SelectResponse = await GetPageSelectData();
            return View(new IframeTransferData<TemplateTypeEditResponse>
            {
                Data = response,
                Id = id.ToString()
            });
        }
        /// <summary>
        /// 修改基础模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(TemplateTypeEditRequest request)
        {
            var input = _mapper.Map<TemplateTypeEditInput>(request);
            return ApiJson(new ApiResult { Success = await _service.EditAsync(input) });
        }

        /// <summary>
        /// 导出语言模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModelStateValidationFilter]
        [FileDownload]
        public async Task<IActionResult> Export([NotEmpty, FromQuery]long id)
        {
            var (fileContent, fileName) = await _service.ExportAsync(id);
            return FileHtml(fileContent, fileName);
        }

        [HttpGet]
        public IActionResult Import()
        {
            TemplateSelectResponse res = new TemplateSelectResponse();
            res.Languages = LanguageHelper.GetLanguageList();
            return View(res);
        }

        /// <summary>
        /// 导入语言模板
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Import(TemplateImportRequest request)
        {
            string jsonData = string.Empty;
            using (StreamReader reader = new StreamReader(request.FormFile.OpenReadStream()))
            {
                jsonData = reader.ReadToEnd();
            }
            var output = await _service.ImportAsync(jsonData, request.Language);

            return ApiJson(new ApiResult<ImportShowOutput> { Success = output != null, Data = output });
        }

        /// <summary>
        /// 批量更新语言模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> UpdateTemplate([NotEmpty, FromQuery]long id)
        {
            bool result = await _service.UpdateTemplateAsync(id);
            string msg = result ? "操作成功" : "操作失败：语言模板不存在";
            return ApiJson(new ApiResult { Success = result, Msg = msg });
        }

        /// <summary>
        /// 基础模板预览
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Preview([NotEmpty, FromQuery]long id)
        {
            try
            {
                TemplateShowResponse res = _mapper.Map<TemplateShowResponse>(await _service.PreviewAsync(id));
                return View(res);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ValidateParameterFailed", "Home", new { Error = ex.Message });
            }
        }

        /// <summary>
        /// 语言模板预览
        /// </summary>
        /// <param name="id"></param>
        /// <param name="loadInCache">是否从缓存加载</param>
        /// <returns></returns>
        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> TemplatePreview([NotEmpty, FromQuery]long id, [FromQuery]bool loadInCache = false)
        {
            try
            {
                TemplateShowResponse res = _mapper.Map<TemplateShowResponse>(await _service.TemplatePreviewAsync(id, loadInCache));
                return View("Preview", res);
            }
            catch (Exception ex)
            {
                return RedirectToAction("ValidateParameterFailed", "Home", new { Error = ex.Message });
            }
        }
    }
}