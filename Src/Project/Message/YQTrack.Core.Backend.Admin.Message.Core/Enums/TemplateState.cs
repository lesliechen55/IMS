using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Message.Core.Enums
{
    /// <summary>
    /// 模版状态
    /// </summary>
    public enum TemplateState
    {
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disable = 0,

        /// <summary>
        /// 启用
        /// </summary>
        [Description("启用")]
        Enable = 1
    }
}
