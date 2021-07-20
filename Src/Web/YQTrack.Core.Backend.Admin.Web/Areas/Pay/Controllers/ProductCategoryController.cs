using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    public class ProductCategoryController : BasePayController
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;

        public ProductCategoryController(IProductCategoryService productCategoryService,
            IMapper mapper)
        {
            _productCategoryService = productCategoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetPageData(ProductCategoryPageDataRequest request)
        {
            var input = _mapper.Map<ProductCategoryPageDataInput>(request);
            var (outputs, total) = await _productCategoryService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<ProductCategoryPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<ProductCategoryPageDataResponse>
            {
                Data = data,
                Count = total
            });
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new IframeTransferData());
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add(ProductCategoryAddRequest request)
        {
            var input = _mapper.Map<ProductCategoryAddInput>(request);
            await _productCategoryService.AddAsync(input, LoginManager.Id);
            return ApiJson();
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([NotEmpty, FromQuery]long id)
        {
            var output = await _productCategoryService.GetByIdAsync(id);
            var response = _mapper.Map<ProductCategoryResponse>(output);
            return View(new IframeTransferData<ProductCategoryResponse>
            {
                Id = id.ToString(),
                Data = response
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(ProductCategoryEditRequest request)
        {
            await _productCategoryService.EditAsync(request.Id, request.Name, request.Code, request.Desc, LoginManager.Id);
            return ApiJson();
        }
    }
}