using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Core.Enum
{
    public enum OperationType
    {
        [Description("查询")]
        Query = 0,

        [Description("添加")]
        Add = 1,

        [Description("修改")]
        Edit = 2,

        [Description("删除")]
        Delete = 3
    }
}