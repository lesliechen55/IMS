using System.ComponentModel;
using OfficeOpenXml.Style;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response
{
    [ExcelSheet(Name = "最近7天报价有更新的运输商", RowHeight = 30)]
    public class ExportCarrierResponse
    {
        [DisplayName("公司编号")]
        [ExcelColumn(Sort = 0)]
        public string CompanyId { get; set; }

        [DisplayName("公司名称")]
        [ExcelColumn(HorizontalStyle = ExcelHorizontalAlignment.Left, Sort = 1)]
        public string CompanyName { get; set; }
    }
}