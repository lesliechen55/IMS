using System;
using YQTrack.Core.Backend.Admin.Core.Enum;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response
{
    public class PermissionResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string AreaName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string FullName { get; set; }

        public string Url { get; set; }

        public int ParentId { get; set; }

        public int Sort { get; set; }

        public string Remark { get; set; }

        public string Icon { get; set; }

        public MenuType MenuType { get; set; }

        public string MenuTypeDesc { get; set; }

        public string TopMenuKey { get; set; }

        public bool IsMultiAction { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? UpdatedTime { get; set; }
    }
}