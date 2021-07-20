using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Freight
{
    public enum FreightType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        [Description("未定义")]
        None = 0,

        /// <summary>
        /// 首续重
        /// </summary>
        [Description("区间续重")]
        FirstPrice = 1,

        /// <summary>
        /// 一口价
        /// </summary>
        [Description("一口价")]
        FixedPrice = 2,

        /// <summary>
        /// 不计首重
        /// </summary>
        [Description("区间计重")]
        NotFirstPrice = 3,
    }
}