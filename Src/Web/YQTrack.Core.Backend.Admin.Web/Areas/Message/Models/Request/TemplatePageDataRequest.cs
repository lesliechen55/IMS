using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request
{
    /// <summary>
    /// 语言模板列表搜索条件
    /// </summary>
    public class TemplatePageDataRequest : PageRequest
    {
        /// <summary>
        /// 模版类型Id
        /// </summary>
        public long TemplateTypeId { get; set; }
        /// <summary>
        /// 模板语言
        /// </summary>
        public string Language { get; set; }
    }
}
