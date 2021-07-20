using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class OfflinePaymentPageDataInput : PageInput
    {
        /// <summary>
        /// 注册邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public OfflineAuditStatus? Status { get; set; }
    }
}
