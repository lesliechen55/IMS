using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Seller.DTO.Input;
using YQTrack.Core.Backend.Admin.Seller.Service;
using YQTrack.Core.Backend.Admin.User.DTO;
using YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Response;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Seller.Controllers
{
    public class BatchTaskController : BaseSellerController
    {
        private readonly ITrackBatchTaskService _trackBatchTaskService;
        private readonly IMapper _mapper;

        public BatchTaskController(ITrackBatchTaskService trackBatchTaskService, IMapper mapper)
        {
            _trackBatchTaskService = trackBatchTaskService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string userRoute)
        {
            return View(new BatchTaskSelectDataResponse()
            {
                UserRouteDto = JsonConvert.DeserializeObject<UserRouteDto>(userRoute)
            });
        }

        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        [ResultEncodeFilter]
        public async Task<IActionResult> GetPageData(BatchTaskPageDataRequest request)
        {
            var input = _mapper.Map<BatchTaskPageDataInput>(request);
            var (outputs, total) = await _trackBatchTaskService.GetPageDataAsync(input);
            var data = _mapper.Map<IEnumerable<BatchTaskPageDataResponse>>(outputs);
            return MyPageJson(data, total);
        }
    }
}