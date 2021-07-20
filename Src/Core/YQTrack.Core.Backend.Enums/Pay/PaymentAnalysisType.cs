using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Pay
{
    /// <summary>
    /// 交易统计分析类型
    /// </summary>
    public enum PaymentAnalysisType
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
        /// 按照支付渠道
        /// </summary>
        [Description("按照支付渠道")]
        PaymentProvider = 3,

        /// <summary>
        /// 按照币种 
        /// </summary>
        [Description("按照币种")]
        CurrencyType = 4,

        /// <summary>
        /// 按照交易状态
        /// </summary>
        [Description("按照交易状态")]
        PaymentStatus = 5
    }
}
