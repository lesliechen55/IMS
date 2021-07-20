using System;

namespace YQTrack.Core.Backend.Admin.Web.Common
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ExcelSheetAttribute : Attribute
    {
        /// <summary>
        /// 工作表名称
        /// </summary>
        public string Name { get; set; } = "Sheet1";

        /// <summary>
        /// 表格行高
        /// </summary>
        public double RowHeight { get; set; } = 28;
    }
}