using System;
using System.ComponentModel;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response
{
    [ExcelSheet(Name = "渠道信息")]
    public class ChannelPageDataResponse
    {
        [DisplayName("渠道名称")]
        public string ChannelTitle { get; set; }

        [DisplayName("渠道ID")]
        public string ChannelId { get; set; }

        [DisplayName("产品类型")]
        public string ProductType { get; set; }

        [DisplayName("最小天数")]
        public byte MinDay { get; set; }

        [DisplayName("最大天数")]
        public byte MaxDay { get; set; }

        [DisplayName("揽件城市")]
        public string Citys { get; set; }

        [DisplayName("派送国家")]
        public string Countrys { get; set; }

        [DisplayName("限重")]
        public int LimitWeight { get; set; }

        [DisplayName("操作费")]
        public decimal OperateCost { get; set; }

        [DisplayName("首重(g)")]
        public int FirstWeight { get; set; }

        [DisplayName("首重价格")]
        public decimal FirstPrice { get; set; }

        [DisplayName("报价类型")]
        public string FreightType { get; set; }

        [DisplayName("报价区间")]
        public string FreightIntervals { get; set; }

        [DisplayName("运输商")]
        public string CompanyName { get; set; }

        [DisplayName("发布时间")]
        [ExcelDateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime? PublishTime { get; set; }

        [DisplayName("渠道状态")]
        public string State { get; set; }

        [DisplayName("过期时间")]
        [ExcelDateTimeFormat("yyyy-MM-dd HH:mm:ss")]
        public DateTime ExpireTime { get; set; }

        [DisplayName("有效举报次数")]
        public int ValidReportTimes { get; set; }
    }
}