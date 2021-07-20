using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace YQTrack.Core.Backend.Admin.Web.Models.Request
{
    public class UpdateAvatarRequest
    {
        [DisplayName("上传文件")]
        public IFormFile FormFile { get; set; }
    }
}