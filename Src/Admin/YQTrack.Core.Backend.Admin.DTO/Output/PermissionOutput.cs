using System;
using YQTrack.Core.Backend.Admin.Core.Enum;

namespace YQTrack.Core.Backend.Admin.DTO.Output
{
    public class PermissionOutput
    {
        public int FId { get; set; }

        public string FName { get; set; }

        public string FAreaName { get; set; }

        public string FControllerName { get; set; }

        public string FActionName { get; set; }

        public string FFullName { get; set; }

        public string FUrl { get; set; }

        public int? FParentId { get; set; }

        public int FSort { get; set; }

        public string FRemark { get; set; }

        public string FIcon { get; set; }

        public MenuType FMenuType { get; set; }

        public string FTopMenuKey { get; set; }

        public bool? FIsMultiAction { get; set; }

        public DateTime FCreatedTime { get; set; }
        public DateTime? FUpdatedTime { get; set; }
    }
}