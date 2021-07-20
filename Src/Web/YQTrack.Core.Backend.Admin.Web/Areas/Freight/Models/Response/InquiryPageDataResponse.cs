using System;
using System.ComponentModel;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response
{
    [ExcelSheet(Name = "询价单信息")]
    public class InquiryPageDataResponse
    {
        [DisplayName("询价单ID")]
        public string Id { get; set; }

        [DisplayName("标题")]
        public string Title { get; set; }

        [DisplayName("询价单号")]
        public string InquiryNo { get; set; }

        [DisplayName("用户ID")]
        public string UserId { get; set; }

        [DisplayName("用户短标识")]
        public string UserUniqueId { get; set; }

        [DisplayName("揽件城市")]
        public string PackageCity { get; set; }

        [DisplayName("派送国家")]
        public string DeliveryCountry { get; set; }

        [DisplayName("发布时间")]
        [ExcelDateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime PublishDateTime { get; set; }

        [DisplayName("状态")]
        public string Status { get; set; }

        [DisplayName("处理时间")]
        [ExcelDateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime? ProcessTime { get; set; }

        [DisplayName("物流要求")]
        public string LogisticsRequire { get; set; }

        [DisplayName("联系方式")]
        public string ContactInfo { get; set; }

        [DisplayName("竞价数")]
        public int QuoterCount { get; set; }

        [DisplayName("浏览数")]
        public int ViewerCount { get; set; }

        [DisplayName("截止时间")]
        [ExcelDateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime ExpireDate { get; set; }

        [DisplayName("状态变更时间")]
        [ExcelDateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime StatusTime { get; set; }
    }
}