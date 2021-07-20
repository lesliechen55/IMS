using System;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class ProductSkuPageDataOutput
    {
        public long FProductSkuId { get; set; }
        public string FName { get; set; }
        public string FCode { get; set; }
        public string FDescription { get; set; }
        public bool FActive { get; set; }
        public UserMemberLevel FMemberLevel { get; set; }
        public DateTime FCreateAt { get; set; }
        public DateTime FUpdateAt { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 商品分类名称
        /// </summary>
        public string ProductCategoryName { get; set; }

        /// <summary>
        /// Sku价格数量
        /// </summary>
        public int PriceCount { get; set; }

        /// <summary>
        /// Sku类型
        /// </summary>
        public SkuType SkuType { get; set; }

        public bool IsInternalUse { get; set; }
    }
}