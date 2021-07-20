using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Backend.Enums;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    public class ProductSkuController : BasePayController
    {
        private readonly IMapper _mapper;
        private readonly IProductSkuService _productSkuService;

        public ProductSkuController(IMapper mapper,
            IProductSkuService productSkuService)
        {
            _mapper = mapper;
            _productSkuService = productSkuService;
        }

        #region 主页

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _productSkuService.GetAllProductAsync();
            return View(products);
        }

        [HttpPost]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetPageData(ProductSkuPageDataRequest request)
        {
            var input = _mapper.Map<ProductSkuPageDataInput>(request);
            var (outputs, total) = await _productSkuService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<ProductSkuPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<ProductSkuPageDataResponse>
            {
                Data = data,
                Count = total
            });
        }


        #endregion

        #region Add

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var products = await _productSkuService.GetAllProductAsync();
            return View(new IframeTransferData<Dictionary<long, string>>
            {
                Data = products
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Add(ProductSkuAddRequest request)
        {
            var input = _mapper.Map<ProductSkuAddInput>(request);
            await _productSkuService.AddAsync(input, LoginManager.Id);
            return ApiJson();
        }

        #endregion

        #region Edit

        /// <summary>
        /// 获取基本SKU信息渲染修改视图
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit([NotEmpty, FromQuery]long id)
        {
            var output = await _productSkuService.GetEditOutputAsync(id);
            var response = _mapper.Map<ProductSkuEditResponse>(output);
            response.AllProductDic = await _productSkuService.GetAllProductAsync();
            return View(new IframeTransferData<ProductSkuEditResponse>
            {
                Id = id.ToString(),
                Data = response
            });
        }

        /// <summary>
        /// 获取SKU价格信息
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Edit))]
        public async Task<IActionResult> GetPriceList([NotEmpty]long skuId)
        {
            var outputs = await _productSkuService.GetPriceListAsync(skuId);
            var response = _mapper.Map<IEnumerable<ProductSkuPriceResponse>>(outputs).ToList();
            return new JsonResult(new PageResponse<ProductSkuPriceResponse>
            {
                Data = response,
                Count = response.Count
            });
        }

        /// <summary>
        /// 获取SKU业务控制信息
        /// </summary>
        /// <param name="skuId"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Edit))]
        public async Task<IActionResult> GetBusinessList([NotEmpty]long skuId)
        {
            var outputs = await _productSkuService.GetBusinessListAsync(skuId);
            var response = _mapper.Map<IEnumerable<ProductSkuBusinessResponse>>(outputs).ToList();
            return new JsonResult(new PageResponse<ProductSkuBusinessResponse>
            {
                Data = response,
                Count = response.Count
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Edit(ProductSkuEditRequest request)
        {
            var input = _mapper.Map<ProductSkuEditInput>(request);
            await _productSkuService.EditAsync(input, LoginManager.Id);
            return ApiJson();
        }

        #endregion

        #region Prcie

        [HttpGet]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Edit))]
        public async Task<IActionResult> AddPrice([NotEmpty, FromQuery]long id, [Required(AllowEmptyStrings = false), FromQuery]string tableId)
        {
            return View(await Task.FromResult(new IframeTransferData
            {
                Id = id.ToString(),
                InvokeElementId = tableId
            }));
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Edit))]
        public async Task<IActionResult> AddPrice(ProductSkuAddPriceRequest request)
        {
            var input = _mapper.Map<ProductSkuAddPriceInput>(request);
            await _productSkuService.AddPriceAsync(input, LoginManager.Id);
            return ApiJson();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Edit))]
        public async Task<IActionResult> DeletePrice([NotEmpty, FromForm] long skuId, [NotEmpty, FromForm] long priceId)
        {
            await _productSkuService.DeletePriceAsync(skuId, priceId, LoginManager.Id);
            return ApiJson();
        }

        #endregion

        /// <summary>
        /// 修改SKU状态(启用或者禁用)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        public async Task<IActionResult> ChangeStatus(ChangeStatusRequest request)
        {
            // ReSharper disable once PossibleInvalidOperationException
            await _productSkuService.ChangeStatusAsync(request.SkuId, request.Enable.Value, LoginManager.Id);
            return ApiJson();
        }

        #region Business

        [HttpGet]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Edit))]
        public async Task<IActionResult> AddBusiness([NotEmpty, FromQuery]long id)
        {
            return View(await Task.FromResult(new IframeTransferData
            {
                Id = id.ToString()
            }));
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Edit))]
        public async Task<IActionResult> AddBusiness(ProductSkuAddBusinessRequest request)
        {
            var input = _mapper.Map<ProductSkuAddBusinessInput>(request);
            await _productSkuService.AddBusinessAsync(input, LoginManager.Id);
            return ApiJson();
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Edit))]
        public async Task<IActionResult> DeleteBusiness([NotEmpty, FromForm] long skuId, [NotEmpty, FromForm] BusinessCtrlType businessCtrlType)
        {
            await _productSkuService.DeleteBusinessAsync(skuId, businessCtrlType, LoginManager.Id);
            return ApiJson();
        }

        #endregion

    }
}