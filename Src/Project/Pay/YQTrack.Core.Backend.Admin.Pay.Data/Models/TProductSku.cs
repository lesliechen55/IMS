using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TProductSku
    {
        public TProductSku()
        {
            TProductSkuPrice = new HashSet<TProductSkuPrice>();
        }

        public long FProductSkuId { get; set; }
        public long FProductId { get; set; }
        public string FName { get; set; }
        public string FDescription { get; set; }
        public bool FActive { get; set; }
        public string FCode { get; set; }
        public UserMemberLevel FMemberLevel { get; set; }
        public string FBusiness { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }

        public virtual TProduct FProduct { get; set; }
        public virtual ICollection<TProductSkuPrice> TProductSkuPrice { get; set; }

        /// <summary>
        /// Sku类型
        /// </summary>
        public SkuType FType { get; set; }

        /// <summary>
        /// 标记是否内部使用(如果是内部使用,则表示用户不能主动下单此SKU)
        /// </summary>
        public bool FIsInternalUse { get; set; }
    }
}
