namespace YQTrack.Core.Backend.Admin.Message.DTO.Output
{
    /// <summary>
    /// 导入词条返回数据
    /// </summary>
    public class ImportShowOutput
    {
        /// <summary>
        /// 基础模板ID
        /// </summary>
        public string TemplateTypeId { get; set; }
        /// <summary>
        /// 语言模板ID
        /// </summary>
        public string TemplateId { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }
    }
}
