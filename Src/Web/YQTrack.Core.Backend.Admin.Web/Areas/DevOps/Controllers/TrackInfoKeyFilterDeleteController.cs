using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.DevOps.DTO;
using YQTrack.Core.Backend.Admin.DevOps.Service;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Areas.DevOps.Controllers
{
    public class TrackInfoKeyFilterDeleteController : BaseDevOpsController
    {
        private readonly ITrackInfoKeyFilterDeleteService _trackInfoKeyFilterDeleteService;

        public TrackInfoKeyFilterDeleteController(ITrackInfoKeyFilterDeleteService trackInfoKeyFilterDeleteService)
        {
            _trackInfoKeyFilterDeleteService = trackInfoKeyFilterDeleteService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var (batchDeleteState, filter) = _trackInfoKeyFilterDeleteService.GetBatchDeleteState();
            ViewBag.AllowDelete = batchDeleteState != BatchDeleteState.Running && batchDeleteState != BatchDeleteState.Cancelling;
            ViewBag.Filter = filter;
            return View();
        }

        [HttpGet]
        [PermissionCode(nameof(Index))]
        public IActionResult GetBatchDeleteState(string key)
        {
            var (batchDeleteState, deleteData) = _trackInfoKeyFilterDeleteService.GetBatchDeleteKeys(key);
            bool state = batchDeleteState == BatchDeleteState.Completed || batchDeleteState == BatchDeleteState.Cancelled;
            return MyJson((state, deleteData));
        }

        [HttpGet]
        [ModelStateValidationFilter]
        public IActionResult BatchDelete([FromQuery, Required(AllowEmptyStrings = false)]string filter)
        {
            if (filter.Length < 4)
            {
                throw new BusinessException("匹配规则不能少于四个字符");
            }
            _trackInfoKeyFilterDeleteService.BatchDelete(filter);
            return ApiJson();
        }

        [HttpGet]
        [PermissionCode(nameof(CancelBatchDelete))]
        public IActionResult CancelBatchDelete()
        {
            _trackInfoKeyFilterDeleteService.CancelBatchDelete();
            return ApiJson();
        }
    }
}