namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response
{
    /// <summary>
    /// 基础模板编辑
    /// </summary>
    public class TemplateTypeEditResponse
    {
        /// <summary>
        /// 基础模板ID
        /// </summary>
        public long TemplateTypeId { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public long ProjectId { get; set; }
        /// <summary>
        /// 发送通道
        /// </summary>
        public int Channel { get; set; }
        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }
        /// <summary>
        /// 模板描述
        /// </summary>
        public string TemplateDescribe { get; set; }
        /// <summary>
        /// 样例数据
        /// </summary>
        public string DataJson { get; set; }
        /// <summary>
        /// 模板标题
        /// </summary>
        public string TemplateTitle { get; set; }
        /// <summary>
        /// 模板内容
        /// </summary>
        public string TemplateBody { get; set; }

        /// <summary>
        /// 下拉框数据
        /// </summary>
        public TemplateTypeSelectResponse SelectResponse { get; set; }

    }
}
