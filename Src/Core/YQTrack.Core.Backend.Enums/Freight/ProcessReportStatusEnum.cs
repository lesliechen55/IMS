using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Freight
{
    public enum ProcessReportStatusEnum
    {
        [Description("待处理")]
        WaitProcess = 0,

        [Description("有效举报")]
        ValidReport = 1,

        [Description("无效举报")]
        InvalidReport = 2
    }
}