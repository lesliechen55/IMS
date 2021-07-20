using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Seller
{
    /// <summary>
    /// 用户店铺状态
    /// </summary>
    public enum ShopStateType : byte
    {
        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enabled = 0,

        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disable = 1,

        /// <summary>
        /// 删除
        /// </summary>
        [Description("删除")]
        Delete = 2,

        /// <summary>
        /// 授权失效（包括过期）
        /// </summary>
        [Description("授权失效")]
        Expire = 3,
    }
}