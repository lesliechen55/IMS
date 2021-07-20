using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ImportGlocashRequest
    {
        [DisplayName("Json文件")]
        public IFormFile FormFile { get; set; }

        [DisplayName("年份")]
        public int? Year { get; set; }

        [DisplayName("年份")]
        public int? Month { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }
    }
}