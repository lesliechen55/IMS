using System.ComponentModel;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ProductSkuAddRequest
    {
        [DisplayName("产品类型ID")]
        public long ProductId { get; set; }

        [DisplayName("会员类型")]
        public UserMemberLevel? MemberLevel { get; set; }

        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("编码")]
        public string Code { get; set; }

        [DisplayName("描述")]
        public string Desc { get; set; }

        [DisplayName("Sku类型")]
        public SkuType? SkuType { get; set; }

        [DisplayName("是否内部使用")]
        public bool? IsInternalUse { get; set; }
    }
}