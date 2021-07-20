using System.Collections.Generic;
using YQTrack.Backend.Message.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response
{
    public class UserRoleTypeResponse
    {
        /// <summary>
        /// 用户角色类型
        /// </summary>
        public List<UserRoleType> UserRoleTypes { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 发送渠道
        /// </summary>
        public ChannelSend Channel { get; set; }
    }

    /// <summary>
    /// 用户角色类型
    /// </summary>
    public class UserRoleType
    {
        /// <summary>
        /// 发送渠道Id
        /// </summary>
        public int UserRoleTypeId { get; set; }

        /// <summary>
        /// 渠道名称
        /// </summary>
        public string UserRoleTypeName { get; set; }
    }
}
