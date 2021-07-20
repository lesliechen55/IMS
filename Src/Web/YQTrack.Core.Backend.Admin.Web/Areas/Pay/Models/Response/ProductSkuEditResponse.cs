using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ProductSkuEditResponse
    {
        public string ProductSkuId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        /// <summary>
        /// SKU对应商品
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 会员类型
        /// </summary>
        public int MemberLevel { get; set; }

        public int Type { get; set; }

        public Dictionary<long, string> AllProductDic { get; set; }

        public bool IsInternalUse { get; set; }
    }
}