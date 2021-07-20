using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.DevOps.DTO;
using YQTrack.Core.Backend.Admin.DevOps.Service;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.DevOps.Controllers
{
    public class TrackInfoNumbersDeleteController : BaseDevOpsController
    {
        private readonly ITrackInfoNumbersDeleteService _trackInfoNumbersDeleteService;

        public TrackInfoNumbersDeleteController(ITrackInfoNumbersDeleteService trackInfoNumbersDeleteService)
        {
            _trackInfoNumbersDeleteService = trackInfoNumbersDeleteService;
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
        public IActionResult GetListTrackCache([FromQuery, Required(AllowEmptyStrings = false)]string trackNos)
        {
            List<TrackCache> list = null;
            try
            {
                string[] numbers = trackNos.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
                if (numbers.Length == 0)
                {
                    throw new BusinessException("单号不能为空");
                }
                list = _trackInfoNumbersDeleteService.GetListTrackCache(numbers);
            }
            catch (System.Exception ex)
            {
                return ApiJson(new PageResponse<TrackCache>() { Code = -1, Success = false, Count = -1, Msg = ex.Message });
            }
            return MyPageJson(list, 1000);
        }

        [HttpPost]
        [ModelStateValidationFilter]
        public IActionResult DeleteKeys([Required(AllowEmptyStrings = false)] string keys)
        {
            string[] arrKey = keys.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Distinct().ToArray();
            if (arrKey.Length == 0)
            {
                throw new BusinessException("请选择要删除的缓存");
            }
            _trackInfoNumbersDeleteService.DeleteKeys(arrKey);
            return ApiJson();
        }
    }
}