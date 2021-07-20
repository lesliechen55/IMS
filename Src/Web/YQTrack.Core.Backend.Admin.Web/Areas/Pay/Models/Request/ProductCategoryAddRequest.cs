using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ProductCategoryAddRequest
    {
        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("编码")]
        public string Code { get; set; }

        [DisplayName("描述")]
        public string Desc { get; set; }
    }
}