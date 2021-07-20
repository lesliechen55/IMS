using YQTrack.Core.Backend.Admin.Message.Core.Enums;

namespace YQTrack.Core.Backend.Admin.Message.DTO.Input
{
    /// <summary>
    /// 发送任务编辑
    /// </summary>
    public class SendTaskEditInput
    {
        /// <summary>
        /// 发送类型：0-按用户 1-按角色 
        /// </summary>
        public SendType SendType { get; set; }

        /// <summary>
        /// 用户角色（选按角色发送时才有用）
        /// </summary>
        public UserRoleTypeEnum UserRoleType { get; set; }

        /// <summary>
        /// 是否立即发送：0-存为草稿 1-立即发送 
        /// </summary>
        public SendAction SendAction { get; set; }

        /// <summary>
        /// Id
        /// </summary>
        public long TaskId { get; set; }

        /// <summary>
        /// 模版Id
        /// </summary>
        public long TemplateTypeId { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 对象详情(一对一)
        /// </summary>
        public string ObjDetails { get; set; }
    }
    
}
