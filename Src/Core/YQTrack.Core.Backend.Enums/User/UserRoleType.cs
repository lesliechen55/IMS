using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace YQTrack.Core.Backend.Enums.User
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")]
    [Flags]
    public enum UserRoleType : byte
    {
        /// <summary>
        /// 未指定
        /// </summary>
        [Description("未指定")]
        None = 0,

        /// <summary>
        /// 管理员
        /// </summary>
        [Description("管理员")]
        Admin = 1,

        /// <summary>
        /// 卖家
        /// </summary>
        [Description("卖家")]
        Seller = 2,

        /// <summary>
        /// 买家
        /// </summary>
        [Description("买家")]
        Buyer = 4,

        /// <summary>
        /// 运输商
        /// </summary>
        [Description("运输商")]
        Carrier = 8,

        /// <summary>
        /// 匿名用户
        /// </summary>
        [Description("匿名用户")]
        Guest = 10,

        /// <summary>
        /// API接口用户
        /// </summary>
        [Description("API接口用户")]
        ApiUser = 16
    }
}
