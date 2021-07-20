using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TProduct
    {
        public TProduct()
        {
            TProductSku = new HashSet<TProductSku>();
        }

        public long FProductId { get; set; }
        public long FProductCategoryId { get; set; }
        public string FName { get; set; }
        public string FCode { get; set; }
        public string FDescription { get; set; }
        public bool FActive { get; set; } = false;
        public UserRoleType FRole { get; set; }
        public ServiceType FServiceType { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }

        public virtual TProductCategory FProductCategory { get; set; }
        public virtual ICollection<TProductSku> TProductSku { get; set; }

        /// <summary>
        /// 标记是否订阅类型产品(例如:buyer用户在ios或者android购买内置订阅产品所用SKU对应的商品)
        /// </summary>
        public bool FIsSubscription { get; set; }
    }
}
