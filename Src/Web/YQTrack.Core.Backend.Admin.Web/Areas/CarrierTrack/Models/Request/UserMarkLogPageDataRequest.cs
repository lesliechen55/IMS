using System.ComponentModel.DataAnnotations;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request
{
    public class UserMarkLogPageDataRequest : PageRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Email { get; set; }
    }
}