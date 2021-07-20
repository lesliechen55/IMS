using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.TrackApi
{
    public enum ApiState:byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        None = 0,

        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Available = 1,

        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Disabled = 2
    }
}
