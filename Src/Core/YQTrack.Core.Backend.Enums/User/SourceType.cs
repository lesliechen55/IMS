using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace YQTrack.Core.Backend.Enums.User
{
    public enum SourceType
    {
        /// <summary>
        ///     网站
        /// </summary>
        [Description("网站")]
        Web = 0,

        /// <summary>
        ///     APP
        /// </summary>
        [Description("APP")]
        APP = 1,
        /// <summary>
        ///     小程序
        /// </summary>
        [Description("小程序")]
        MiniProgram = 2,

        /// <summary>
        ///    安卓
        /// </summary>
        [Description("Android")]
        Android = 11,
        /// <summary>
        ///  苹果
        /// </summary>
        [Description("Apple")]
        Apple = 12,

        /// <summary>
        ///  支付宝小程序
        /// </summary>
        [Description("支付宝小程序")]
        MiniProgram_alipay = 21,
        /// <summary>
        ///  微信小程序
        /// </summary>
        [Description("微信小程序")]
        MiniProgram_weixin = 22,

        /// <summary>
        ///  Shopify
        /// </summary>
        [Description("Shopify")]
        Shopify = 31
    }
}
