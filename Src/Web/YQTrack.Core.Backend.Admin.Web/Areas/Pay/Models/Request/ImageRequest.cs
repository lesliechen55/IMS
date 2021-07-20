using Microsoft.AspNetCore.Http;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ImageRequest
    {
        public long? UserId { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
