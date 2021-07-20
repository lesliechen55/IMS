using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Freight
{
    /// <summary>
    /// 公司审核状态
    /// </summary>
    public enum CompanyAuditState
    {
        /// <summary>
        /// 无认证信息
        /// </summary>
        [Description("无认证信息")]
        NoAuditInfo = -1,

        /// <summary>
        /// 等待审核
        /// </summary>
        [Description("等待审核")]
        WaitAudit = 0,

        /// <summary>
        /// 审核通过
        /// </summary>
        [Description("审核通过")]
        AuditPass = 1,

        /// <summary>
        /// 审核未通过
        /// </summary>
        [Description("审核未通过")]
        AuditFail = 2
    }
}