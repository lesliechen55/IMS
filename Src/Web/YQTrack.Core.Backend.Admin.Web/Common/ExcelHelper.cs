using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using OfficeOpenXml;

namespace YQTrack.Core.Backend.Admin.Web.Common
{
    public static class ExcelHelper
    {
        public static byte[] GenerateExcel<TModel>(IEnumerable<TModel> models) where TModel : class, new()
        {
            var properties = typeof(TModel).GetProperties().Where(x => !x.IsDefined(typeof(ExcelIgnoreAttribute))).ToArray();
            var excelSheetAttribute = typeof(TModel).GetCustomAttribute<ExcelSheetAttribute>() ?? new ExcelSheetAttribute();
            using (var excel = new ExcelPackage())
            {
                var sheet = excel.Workbook.Worksheets.Add(excelSheetAttribute.Name);

                GenerateExcelHeader(sheet, properties, excelSheetAttribute);

                TModel[] items = models.ToArray();
                for (int i = 2; i <= items.Length + 1; i++)
                {
                    TModel current = items[i - 2];
                    sheet.Row(i).Height = excelSheetAttribute.RowHeight;
                    properties = GetSortPropertyInfos(properties);
                    for (int j = 1; j <= properties.Length; j++)
                    {
                        var cellRang = sheet.Cells[i, j];
                        var pro = properties[j - 1];
                        var format = pro.GetCustomAttribute<DisplayFormatAttribute>();
                        if (format != null && typeof(IFormattable).IsAssignableFrom(pro.PropertyType))
                        {
                            cellRang.Value = ((IFormattable)pro.GetValue(current)).ToString(format.DataFormatString, CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            cellRang.Value = pro.GetValue(current);
                        }

                        var excelDateTimeFormatAttribute = pro.GetCustomAttribute<ExcelDateTimeFormatAttribute>();
                        if (excelDateTimeFormatAttribute != null && pro.GetValue(current) != null && (pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(DateTime?)))
                        {
                            cellRang.Value =
                                ((DateTime)pro.GetValue(current)).ToString(excelDateTimeFormatAttribute.Format);
                        }
                        else
                        {
                            cellRang.Value = pro.GetValue(current);
                        }

                        cellRang.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        var excelColumnAttribute = pro.GetCustomAttribute<ExcelColumnAttribute>() ?? new ExcelColumnAttribute();
                        cellRang.Style.HorizontalAlignment = excelColumnAttribute.HorizontalStyle;
                    }
                }

                // 列宽自适应
                sheet.Cells.AutoFitColumns();

                return excel.GetAsByteArray();
            }
        }

        private static void GenerateExcelHeader(ExcelWorksheet sheet, PropertyInfo[] properties, ExcelSheetAttribute excelSheetAttribute)
        {
            sheet.Name = excelSheetAttribute.Name;
            sheet.CustomHeight = true;
            sheet.DefaultRowHeight = excelSheetAttribute.RowHeight;
            sheet.Row(1).Height = excelSheetAttribute.RowHeight;
            properties = GetSortPropertyInfos(properties);
            for (int i = 1; i <= properties.Length; i++)
            {
                PropertyInfo current = properties[i - 1];
                DisplayAttribute display = current.GetCustomAttribute<DisplayAttribute>();
                DisplayNameAttribute displayName = current.GetCustomAttribute<DisplayNameAttribute>();
                string columnName = display?.GetName() ?? displayName?.DisplayName ?? current.Name;
                var cellRang = sheet.Cells[1, i];
                cellRang.Value = columnName;
                cellRang.Style.Font.Bold = true;
                cellRang.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                cellRang.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            }
        }

        private static PropertyInfo[] GetSortPropertyInfos(PropertyInfo[] properties)
        {
            var orders = properties.Where(x => x.IsDefined(typeof(ExcelColumnAttribute))).OrderBy(x => x.GetCustomAttribute<ExcelColumnAttribute>().Sort).ToArray();
            var others = properties.Where(x => !x.IsDefined(typeof(ExcelColumnAttribute)));
            return orders.Concat(others).ToArray();
        }

    }
}