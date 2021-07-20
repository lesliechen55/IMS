using System;

namespace YQTrack.Core.Backend.Admin.Message.Core
{
    /// <summary>
    /// 可回复（使用可回复邮箱发送）
    /// </summary>
    public class ReplyAddressAttribute : Attribute
    {
    }

    /// <summary>
    /// 邮件结果回复到业务
    /// </summary>
    public class ReplyBusinessAttribute : Attribute
    {
    }
}
