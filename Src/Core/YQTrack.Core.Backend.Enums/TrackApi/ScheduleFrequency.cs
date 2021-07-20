using System.ComponentModel;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Enums.TrackApi
{
    /// <summary>
    /// 调度频率
    /// </summary>
    public enum ScheduleFrequency : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        [IMSIgnore]
        None = 0,

        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 50,

        /// <summary>
        /// 高
        /// </summary>
        [Description("高")]
        High = 100
    }
}