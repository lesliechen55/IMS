using System;
using OfficeOpenXml.Style;

namespace YQTrack.Core.Backend.Admin.Web.Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ExcelColumnAttribute : Attribute
    {
        /// <summary>
        /// 水平居中
        /// </summary>
        public ExcelHorizontalAlignment HorizontalStyle { get; set; } = ExcelHorizontalAlignment.Left;

        /// <summary>
        /// 列排序号
        /// </summary>
        public int Sort { get; set; } = 0;
    }
}