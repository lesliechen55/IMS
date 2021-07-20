using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Controllers
{
    public class TemplateController : BaseMessageController
    {
        private readonly IMapper _mapper;
        private readonly ITemplateService _service;

        public TemplateController(IMapper mapper, ITemplateService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult Index()
        {
            TemplateSelectResponse res = new TemplateSelectResponse();
            res.Languages = LanguageHelper.GetLanguageList();
            return View(res);
        }

        /// <summary>
        /// 获取语言模板列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionCode(nameof(Index))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetPageData(TemplatePageDataRequest request)
        {
            var input = _mapper.Map<TemplatePageDataInput>(request);
            var (outputs, total) = await _service.GetPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<TemplatePageDataResponse>>(outputs);
            return ApiJson(new PageResponse<TemplatePageDataResponse>
            {
                Count = total,
                Data = responses
            });
        }


        /// <summary>
        /// 添加语言模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add([NotEmpty, FromQuery]long id)
        {
            return ApiJson(new ApiResult { Success = await _service.AddAsync(id) });
        }

        /// <summary>
        /// 删除语言模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Delete([NotEmpty, FromQuery]long id)
        {
            return ApiJson(new ApiResult { Success = await _service.DeleteAsync(id) });
        }

        /// <summary>
        /// 删除语言模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> RealDelete([NotEmpty, FromQuery]long id)
        {
            await _service.RealDeleteAsync(id);
            return ApiJson(new ApiResult());
        }
    }
}