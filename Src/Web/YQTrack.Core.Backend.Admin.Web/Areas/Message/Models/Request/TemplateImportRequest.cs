using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request
{
    public class TemplateImportRequest
    {
        public IFormFile FormFile { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        [Required(ErrorMessage ="请选择语言")]
        public string Language { get; set; }
    }
}
