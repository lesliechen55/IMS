using System.ComponentModel;
using YQTrack.Core.Backend.Admin.Core.Enum;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request
{
    public class PermissionAddRequest
    {
        [DisplayName("权限别名")]
        public string Name { get; set; }

        [DisplayName("区域名字")]
        public string AreaName { get; set; }

        [DisplayName("控制器名字")]
        public string ControllerName { get; set; }

        [DisplayName("行为名字")]
        public string ActionName { get; set; }

        [DisplayName("代码全称")]
        public string FullName { get; set; }

        [DisplayName("请求地址")]
        public string Url { get; set; }

        [DisplayName("父级菜单ID")]
        public int? ParentId { get; set; }

        [DisplayName("排序号")]
        public int Sort { get; set; }

        [DisplayName("菜单类型")]
        public MenuType? MenuType { get; set; }

        [DisplayName("顶级菜单Key")]
        public string TopMenuKey { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }

        [DisplayName("图标")]
        public string Icon { get; set; }

        public bool? IsMultiAction { get; set; }
    }
}