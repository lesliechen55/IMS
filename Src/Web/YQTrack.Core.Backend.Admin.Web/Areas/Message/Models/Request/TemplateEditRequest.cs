namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request
{
    /// <summary>
    /// 语言模板编辑
    /// </summary>
    public class TemplateEditRequest
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
