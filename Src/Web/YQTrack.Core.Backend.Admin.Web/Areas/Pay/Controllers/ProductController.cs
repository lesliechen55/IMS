using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class ProductController : BasePayController
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService,
            IMapper mapper)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(new IframeTransferData<ProductSelectDataResponse>
            {
                Data = new ProductSelectDataResponse()
                {
                    ListCategory = await _productCategoryService.GetAllDataAsync()
                }
            });
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetPageData(ProductPageDataRequest request)
        {
            var input = _mapper.Map<ProductPageDataInput>(request);
            var (outputs, total) = await _productService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<ProductPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<ProductPageDataResponse>
            {
                Data = data,
                Count = total
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add()
        {
            return View(new IframeTransferData<ProductSelectDataResponse>
            {
                Data = new ProductSelectDataResponse()
                {
                    ListCategory = await _productCategoryService.GetAllDataAsync()
                }
            });
        }

        /// <summary>
        /// 添加商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add(ProductEditRequest request)
        {
            var input = _mapper.Map<ProductEditInput>(request);
            return ApiJson(new ApiResult { Success = await _productService.AddAsync(input, LoginManager.Id) });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([Required(AllowEmptyStrings = false), FromQuery]int id)
        {
            ProductShowResponse response = _mapper.Map<ProductShowResponse>(await _productService.GetByIdAsync(id));
            response.ProductSelectData = new ProductSelectDataResponse()
            {
                ListCategory = await _productCategoryService.GetAllDataAsync()
            };
            return View(new IframeTransferData<ProductShowResponse> { Data = response });
        }

        /// <summary>
        /// 修改商品
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(ProductEditRequest request)
        {
            var input = _mapper.Map<ProductEditInput>(request);
            return ApiJson(new ApiResult { Success = await _productService.EditAsync(input, LoginManager.Id) });
        }

        /// <summary>
        /// 修改商品状态(启用或者禁用)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> ChangeStatus(ChangeProductStatusRequest request)
        {
            // ReSharper disable once PossibleInvalidOperationException
            await _productService.ChangeStatusAsync(request.ProductId, request.Active.Value, LoginManager.Id);
            return ApiJson();
        }
    }
}