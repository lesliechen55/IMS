using System.ComponentModel;
using YQTrack.Core.Backend.Admin.Core.Enum;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request
{
    public class PermissionEditRequest
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AreaName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string FullName { get; set; }

        public string Url { get; set; }

        public int? ParentId { get; set; }

        public int Sort { get; set; }

        public string Remark { get; set; }

        [DisplayName("菜单类型")]
        public MenuType? MenuType { get; set; }

        [DisplayName("顶级菜单Key")]
        public string TopMenuKey { get; set; }

        public string Icon { get; set; }

        public bool? IsMultiAction { get; set; }
    }
}