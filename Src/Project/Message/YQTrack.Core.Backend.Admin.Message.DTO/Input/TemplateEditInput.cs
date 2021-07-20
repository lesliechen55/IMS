namespace YQTrack.Core.Backend.Admin.Message.DTO.Input
{
    /// <summary>
    /// 语言模板编辑
    /// </summary>
    public class TemplateEditInput
    {
        /// <summary>
        /// 基础模板ID
        /// </summary>
        public long TemplateTypeId { get; set; }
        /// <summary>
        /// 语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 模板标题
        /// </summary>
        public string TemplateTitle { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        public string TemplateBody { get; set; }
        /// <summary>
        /// 语言条目
        /// </summary>
        public string TemplateData { get; set; }

    }
}
