using System;
using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Message.Core.Enums
{
    /// <summary>
    /// 对象类型
    /// </summary>
    public enum ObjType
    {
        /// <summary>
        /// 默认值
        /// </summary>
        None = -1,

        /// <summary>
        /// 角色
        /// </summary>
        Role = 0,

        /// <summary>
        /// 国家
        /// </summary>
        Country = 1,

        /// <summary>
        /// 语言
        /// </summary>
        Language = 2,

        /// <summary>
        /// 时区
        /// </summary>
        TimeZone = 3,

        /// <summary>
        /// 用户邮箱
        /// </summary>
        ChannelEmail = 4,

        /// <summary>
        /// 用户Token
        /// </summary>
        ChannelApp = 5,

        /// <summary>
        /// 用户站内信ID
        /// </summary>
        ChannelSiteMessage = 6

    }
    /// <summary>
    /// 发送类型
    /// </summary>
    public enum SendType
    {
        /// <summary>
        /// 按用户发送
        /// </summary>
        ByUser = 0,
        /// <summary>
        /// 按角色发送
        /// </summary>
        ByRole = 1
    }

    /// <summary>
    /// 是否立即发送
    /// </summary>
    public enum SendAction
    {
        /// <summary>
        /// 存为草稿
        /// </summary>
        No = 0,
        /// <summary>
        /// 按角色发送
        /// </summary>
        Yes = 1
    }
    /// <summary>
    /// 用户角色
    /// </summary>
    [Flags]
    public enum UserRoleTypeEnum
    {
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All = 0,

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
    }
}
