using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ProductSkuPageDataResponse
    {
        public long ProductSkuId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string MemberLevel { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品分类名称
        /// </summary>
        public string ProductCategoryName { get; set; }

        public int PriceCount { get; set; }

        public string SkuType { get; set; }

        public string IsInternalUse { get; set; }
    }
}