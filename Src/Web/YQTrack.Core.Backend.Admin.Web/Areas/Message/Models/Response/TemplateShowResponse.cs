namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response
{
    public class TemplateShowResponse
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
