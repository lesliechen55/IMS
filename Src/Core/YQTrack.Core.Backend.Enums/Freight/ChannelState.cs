using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Freight
{
    public enum ChannelState
    {
        /// <summary>
        /// 已发布
        /// </summary>
        [Description("已发布")]
        Published = 1,

        /// <summary>
        /// 停止
        /// </summary>
        [Description("停止")]
        Stopped = 2,

        /// <summary>
        /// 失效
        /// </summary>
        [Description("失效")]
        Expired = 3
    }
}