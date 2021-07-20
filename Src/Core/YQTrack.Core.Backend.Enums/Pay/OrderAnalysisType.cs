using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Pay
{
    /// <summary>
    /// 订单统计分析类型
    /// </summary>
    public enum OrderAnalysisType
    {
        /// <summary>
        /// 按照业务分类
        /// </summary>
        [Description("按照业务分类")]
        ServiceType = 1,

        /// <summary>
        /// 按照平台类型
        /// </summary>
        [Description("按照平台类型")]
        PlatformType = 2,

        /// <summary>
        /// 按照币种
        /// </summary>
        [Description("按照币种")]
        CurrencyType = 3,

        /// <summary>
        /// 按照订单状态
        /// </summary>
        [Description("按照订单状态")]
        OrderStatus = 4
    }
}
