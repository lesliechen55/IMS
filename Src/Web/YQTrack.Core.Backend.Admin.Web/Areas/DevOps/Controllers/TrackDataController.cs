using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.DevOps.Service;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.DevOps.Controllers
{
    public class TrackDataController : BaseDevOpsController
    {
        private readonly ITrackInfoMongoDBService _trackInfoMongoDBService;

        public TrackDataController(ITrackInfoMongoDBService trackInfoMongoDBService)
        {
            _trackInfoMongoDBService = trackInfoMongoDBService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        [ResultEncodeFilter]
        public async Task<IActionResult> GetJsonData([FromQuery, Required(AllowEmptyStrings = false)]string number)
        {
            var (success, json) = await _trackInfoMongoDBService.GetJsonDataAsync(number);
            return ApiJson(new ApiResult<object>()
            {
                Success = success,
                Data = json,
                Msg = json
            });
        }
    }
}