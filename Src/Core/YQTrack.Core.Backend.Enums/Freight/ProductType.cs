using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Freight
{
    public enum ProductType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        [Description("未定义")]
        None = 0,

        /// <summary>
        /// 小包平邮
        /// </summary>
        [Description("小包平邮")]
        SmallMail = 1,

        /// <summary>
        /// 小包挂号
        /// </summary>
        [Description("小包挂号")]
        SmallRegistration = 2,

        /// <summary>
        /// EUB
        /// </summary>
        [Description("EUB")]
        EUB = 3,

        /// <summary>
        /// EMS
        /// </summary>
        [Description("EMS")]
        EMS = 4,

        /// <summary>
        /// 大包
        /// </summary>
        [Description("大包")]
        Big = 5,

        /// <summary>
        /// 专线
        /// </summary>
        [Description("专线")]
        SpecialLine = 6,

        /// <summary>
        /// 快递
        /// </summary>
        [Description("快递")]
        Expressage = 7
    }
}