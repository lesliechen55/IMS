using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Pay
{
    public enum ChartDateType
    {
        [Description("按天统计(30天数据)")]
        [DefaultValue(30)]
        Day = 0,

        [Description("按周统计(12周数据)")]
        [DefaultValue(12)]
        Week = 1,

        [Description("按月统计(12个月数据)")]
        [DefaultValue(12)]
        Month = 2
    }
}