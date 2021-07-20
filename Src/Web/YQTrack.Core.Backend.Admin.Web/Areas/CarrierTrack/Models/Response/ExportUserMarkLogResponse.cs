using System;
using System.ComponentModel;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Response
{
    [ExcelSheet(Name = "用户标签使用记录", RowHeight = 30)]
    public class ExportUserMarkLogResponse
    {
        [DisplayName("注册邮箱")]
        public string Email { get; set; }

        [DisplayName("操作描述")]
        public string Description { get; set; }

        [DisplayName("操作内容")]
        public string Detail { get; set; }

        [DisplayName("操作时间")]
        [ExcelDateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime CreateTime { get; set; }
    }
}