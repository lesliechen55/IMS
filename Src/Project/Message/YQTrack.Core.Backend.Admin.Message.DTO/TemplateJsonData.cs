using System.Collections.ObjectModel;

namespace YQTrack.Core.Backend.Admin.Message.DTO
{
    public class TemplateJsonData
    {
        /// <summary>
        /// 模板代码
        /// </summary>
        public int TemplateCode { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        public string TemplateTypeId { get; set; }

        /// <summary>
        /// 标题语言条目
        /// </summary>
        public Collection<Dict> TitleDict { get; set; }

        /// <summary>
        /// 内容语言条目
        /// </summary>
        public Collection<Dict> BodyDict { get; set; }
    }

    /// <summary>
    /// 语言条目
    /// </summary>
    public class Dict
    {
        /// <summary>
        /// ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 原始词条信息
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 翻译后的词条信息
        /// </summary>
        public string Target { get; set; }
    }
}
