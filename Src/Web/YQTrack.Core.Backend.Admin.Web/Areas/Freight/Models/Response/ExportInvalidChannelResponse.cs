using System.ComponentModel;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response
{
    [ExcelSheet(Name = "查询失效(自然过期或者停用)渠道信息,时间范围本周")]
    public class ExportInvalidChannelResponse
    {
        [DisplayName("渠道标题")]
        public string FchannelTitle { get; set; }

        [DisplayName("产品类型")]
        public byte FproductType { get; set; }

        [DisplayName("最小天数")]
        public byte FminDay { get; set; }

        [DisplayName("最大天数")]
        public byte FmaxDay { get; set; }

        [DisplayName("国家")]
        public string Fcountrys { get; set; }

        [DisplayName("限重")]
        public int FlimitWeight { get; set; }

        [DisplayName("操作费")]
        public decimal FoperateCost { get; set; }

        [DisplayName("首重")]
        public int FfirstWeight { get; set; }

        [DisplayName("首重价格")]
        public decimal FfirstPrice { get; set; }

        [DisplayName("区间Json")]
        public string FfreightIntervals { get; set; }

        [DisplayName("公司名字")]
        public string FcompanyName { get; set; }

        [DisplayName("报价类型")]
        public byte FfreightType { get; set; }

        [DisplayName("失效时间(如果是自动失效)")]
        public string FexpireTime { get; set; }

        [DisplayName("失效方式")]
        public string FexpireType { get; set; }

        [DisplayName("渠道类型")]
        public string FChannelType { get; set; }

        [DisplayName("渠道子类型")]
        public int FchannelSubTypeId { get; set; }

        [DisplayName("发布时间")]
        public string FpublishTime { get; set; }
    }
}