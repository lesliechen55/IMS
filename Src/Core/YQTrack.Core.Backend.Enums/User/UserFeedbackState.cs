using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.User
{
    public enum UserFeedbackState
    {
        /// <summary>
        /// 未处理
        /// </summary>
        [Description("未处理")]
        UnHandle = 0,

        /// <summary>
        /// 已回复
        /// </summary>
        [Description("已回复")]
        Handled = 1
    }
}