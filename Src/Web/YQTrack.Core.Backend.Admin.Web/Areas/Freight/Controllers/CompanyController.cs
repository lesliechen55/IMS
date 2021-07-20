using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Controllers
{
    public class CompanyController : BaseFreightController
    {
        private readonly IMapper _mapper;
        private readonly ICompanyService _companyService;

        public CompanyController(IMapper mapper,
                                 ICompanyService companyService)
        {
            _mapper = mapper;
            _companyService = companyService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> GetCompanyPageData(CompanyPageDataRequest request)
        {
            var input = _mapper.Map<CompanyPageDataInput>(request);
            var (outputs, total) = await _companyService.GetCompanyPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<CompanyPageDataResponse>>(outputs);
            return ApiJson(new PageResponse<CompanyPageDataResponse>
            {
                Count = total,
                Data = responses
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> ViewCheck([NotEmpty, FromQuery]long id)
        {
            var output = await _companyService.GetCompanyViewCheckInfoAsync(id);
            var response = _mapper.Map<CompanyViewCheckResponse>(output);
            return View(new IframeTransferData<CompanyViewCheckResponse>
            {
                Data = response,
                Id = id.ToString(),
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(ViewCheck))]
        public async Task<IActionResult> GetViewCheckImage([Required(AllowEmptyStrings = false), FromQuery]string imgUrl)
        {
            var bytes = await _companyService.GetViewCheckImageAsync(imgUrl);
            return FileImage(bytes);
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([NotEmpty, FromQuery]long id)
        {
            var output = await _companyService.GetCompanyEditInfoAsync(id);
            var response = _mapper.Map<CompanyEditResponse>(output);
            return View(new IframeTransferData<CompanyEditResponse>
            {
                Data = response,
                Id = id.ToString()
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(CompanyEditRequest request)
        {
            var input = _mapper.Map<CompanyEditInput>(request);
            await _companyService.EditAsync(input);
            return ApiJson();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Pass([NotEmpty]long id)
        {
            await _companyService.PassAsync(id);
            return ApiJson();
        }

        [HttpPost]
        [PermissionCode(nameof(Pass))]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Reject([NotEmpty]long id, [Required(AllowEmptyStrings = false)]string desc)
        {
            await _companyService.RejectAsync(id, desc);
            return ApiJson();
        }
    }
}