using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Input
{
    public class InvoiceApplyPageDataInput : PageInput
    {
        /// <summary>
        /// 注册邮箱
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public InvoiceAuditStatus? Status { get; set; }
    }
}
