using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace YQTrack.Core.Backend.Enums.User
{
    /// <summary>
    ///     用户状态
    /// </summary>
    [SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")]
    [Flags]
    public enum UserState : byte
    {
        /// <summary>
        ///     已注册未选择角色
        /// </summary>
        [Description("已注册未选择角色")]
        None = 0,

        /// <summary>
        ///     正常
        /// </summary>
        [Description("正常")]
        Normal = 1,

        /// <summary>
        ///     禁用
        /// </summary>
        [Description("禁用")]
        Disabled = 2
    }
}
