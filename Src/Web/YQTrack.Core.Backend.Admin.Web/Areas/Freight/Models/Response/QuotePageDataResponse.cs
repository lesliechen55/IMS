using System;
using System.ComponentModel;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response
{
    [ExcelSheet(Name = "竞价单数据")]
    public class QuotePageDataResponse
    {
        [DisplayName("报价ID")]
        public string QuoteId { get; set; }

        [DisplayName("竞价单号")]
        public string QuoteOrderNo { get; set; }

        [DisplayName("询价ID")]
        public string OrderId { get; set; }

        [DisplayName("询价单号")]
        public string InquiryOrderNo { get; set; }

        [DisplayName("揽件城市")]
        public int PackageCity { get; set; }

        [DisplayName("派送国家")]
        public int DeliveryCountry { get; set; }

        [DisplayName("询价单状态")]
        public string Status { get; set; }

        [DisplayName("用户ID")]
        public string UserId { get; set; }

        [DisplayName("公司ID")]
        public string CompanyId { get; set; }

        [DisplayName("公司名称")]
        public string CompanyName { get; set; }

        [DisplayName("报价内容")]
        public string Content { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }

        [DisplayName("是否撤销")]
        public bool Cancel { get; set; }

        [DisplayName("撤销时间")]
        [ExcelDateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime? CancelTime { get; set; }

        [DisplayName("撤销原因")]
        public string CancelReason { get; set; }

        [DisplayName("竞价时间")]
        [ExcelDateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime CreateTime { get; set; }

        [DisplayName("Seller是否查看")]
        public bool Viewed { get; set; }
    }
}