using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class ProductSkuEditOutput
    {
        public long FProductSkuId { get; set; }
        public string FName { get; set; }
        public string FCode { get; set; }
        public string FDescription { get; set; }
        public bool FActive { get; set; }
        /// <summary>
        /// SKU对应商品
        /// </summary>
        public long FProductId { get; set; }

        /// <summary>
        /// 会员类型
        /// </summary>
        public UserMemberLevel FMemberLevel { get; set; }

        /// <summary>
        /// Sku类型
        /// </summary>
        public SkuType FType { get; set; }

        public bool FIsInternalUse { get; set; }
    }
}