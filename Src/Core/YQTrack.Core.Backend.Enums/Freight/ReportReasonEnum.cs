using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Freight
{
    /// <summary>
    /// 举报原因枚举
    /// </summary>
    public enum ReportReasonEnum
    {
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 0,

        /// <summary>
        /// 价格与实际不符
        /// </summary>
        [Description("价格与实际不符")]
        PriceNotMatchActual = 1, // 前端传递的默认值

        /// <summary>
        /// 时效与实际不符
        /// </summary>
        [Description("时效与实际不符")]
        TimelinessNotMatchActual = 2,

        /// <summary>
        /// 渠道描述与实际不符
        /// </summary>
        [Description("渠道描述与实际不符")]
        ChannelDescriptionNotMatchActual = 3,

        /// <summary>
        /// 运输属性与实际不符
        /// </summary>
        [Description("运输属性与实际不符")]
        TransportPropertyNotMatchActual = 4
    }
}