using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Seller
{
    /// <summary>
    /// 单号批量处理任务状态
    /// </summary>
    public enum TrackBatchTaskStatus : short
    {
        /// <summary>
        /// 开始处理
        /// </summary>
        [Description("开始处理")]
        Begin = 0,
        /// <summary>
        /// 处理中
        /// </summary>
        [Description("处理中")]
        Processing = 1,
        /// <summary>
        /// 处理完成
        /// </summary>
        [Description("处理完成")]
        Finish = 2

    }
}