using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Core.Enum
{
    /// <summary>
    /// 权限菜单枚举
    /// </summary>
    public enum MenuType
    {
        /// <summary>
        /// 顶级菜单
        /// </summary>
        [Description("顶级菜单")]
        TopMenu = 0,

        /// <summary>
        /// 菜单组
        /// </summary>
        [Description("菜单组")]
        MenuGroup = 1,

        /// <summary>
        /// 功能
        /// </summary>
        [Description("功能")]
        Function = 2
    }
}