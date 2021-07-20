namespace YQTrack.Core.Backend.Admin.Message.DTO.Output
{
    /// <summary>
    /// 语言模板编辑
    /// </summary>
    public class TemplateShowOutput
    {
        /// <summary>
        /// 语言模板ID
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// 模板标题
        /// </summary>
        public string TemplateTitle { get; set; }
        /// <summary>
        /// 模板内容（渲染前）
        /// </summary>
        public string TemplateBody { get; set; }
    }
}
