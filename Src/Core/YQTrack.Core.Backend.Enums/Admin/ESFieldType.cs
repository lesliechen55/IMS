using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Admin
{
    /// <summary>
    /// 字段筛选类型：0-枚举 1-数字范围 2-字符串 3-全文搜索 4-日期范围
    /// </summary>
    public enum ESFieldType : byte
    {
        /// <summary>
        /// 枚举多选
        /// </summary>
        [Description("枚举多选")]
        Enum = 0,

        /// <summary>
        /// 数字范围
        /// </summary>
        [Description("数字范围")]
        NumberRange = 1,

        /// <summary>
        /// 直接输入
        /// </summary>
        [Description("直接输入")]
        InputString = 2,

        /// <summary>
        /// 全文搜索
        /// </summary>
        [Description("全文搜索")]
        Fulltext = 3,

        /// <summary>
        /// 日期范围
        /// </summary>
        [Description("日期范围")]
        DateRange = 4,

        /// <summary>
        /// 布尔类型
        /// </summary>
        [Description("布尔类型")]
        Bool = 5
    }
}
