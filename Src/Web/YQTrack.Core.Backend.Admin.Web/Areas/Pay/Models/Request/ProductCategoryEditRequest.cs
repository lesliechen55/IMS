using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ProductCategoryEditRequest : ProductCategoryAddRequest
    {
        [DisplayName("分类ID")]
        public long Id { get; set; }
    }
}