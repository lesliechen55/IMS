using System.ComponentModel;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class ProductSkuEditInput
    {
        public long ProductSkuId { get; set; }

        [DisplayName("产品类型ID")]
        public long ProductId { get; set; }

        public UserMemberLevel MemberLevel { get; set; }

        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("编码")]
        public string Code { get; set; }

        [DisplayName("描述")]
        public string Desc { get; set; }

        public SkuType SkuType { get; set; }

        public bool IsInternalUse { get; set; }
    }
}