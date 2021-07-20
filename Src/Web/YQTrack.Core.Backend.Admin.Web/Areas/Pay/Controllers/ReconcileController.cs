using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Controllers
{
    public class ReconcileController : BasePayController
    {
        private readonly IReconcileService _reconcileService;
        private readonly IMapper _mapper;

        public ReconcileController(IReconcileService reconcileService, IMapper mapper)
        {
            _reconcileService = reconcileService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();//new ReconcileSelectResponse()
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> GetPageData(ReconcilePageDataRequest requst)
        {
            ReconcilePageDataInput input = _mapper.Map<ReconcilePageDataInput>(requst);
            var (outputs, total) = await _reconcileService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<ReconcilePageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<ReconcilePageDataResponse>
            {
                Data = data,
                Count = total
            });
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public async Task<IActionResult> View([Required(AllowEmptyStrings = false), FromQuery]long id)
        {
            List<ReconcileItemShowResponse> response = _mapper.Map<List<ReconcileItemShowResponse>>(await _reconcileService.GetItemByIdAsync(id));
            response.ForEach(x =>
            {
                if (x.OrderId.Reverse().ToList()[6] == '0')
                {
                    x.IsTestOrder = true;
                }
            });
            response = response.OrderBy(x => x.IsTestOrder).ToList();
            return View(new IframeTransferData<List<ReconcileItemShowResponse>> { Data = response });
        }
    }
}