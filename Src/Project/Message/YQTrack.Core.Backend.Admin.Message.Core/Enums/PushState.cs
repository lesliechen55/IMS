using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Message.Core.Enums
{
    public enum PushState
    {
        /// <summary>
        /// 未发送、准备发送
        /// </summary>
        [Description("准备发送")]
        ReadyPush = 0,

        /// <summary>
        /// 已发送
        /// </summary>
        [Description("已发送")]
        PushFinish = 1,

        /// <summary>
        /// 发送中
        /// </summary>
        [Description("发送中")]
        Pushing = 2,

        /// <summary>
        /// 发送异常
        /// </summary>
        [Description("发送异常")]
        PushError = 4,

        /// <summary>
        /// 草稿状态
        /// </summary>
        [Description("草稿状态")]
        Draft = 5
    }
}
