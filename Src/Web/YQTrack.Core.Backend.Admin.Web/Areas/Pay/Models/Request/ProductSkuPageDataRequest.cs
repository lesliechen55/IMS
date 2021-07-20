using System.ComponentModel;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ProductSkuPageDataRequest : PageRequest
    {
        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("编码")]
        public string Code { get; set; }

        [DisplayName("描述")]
        public string Desc { get; set; }

        [DisplayName("商品名称")]
        public long? ProductType { get; set; }
    }
}