using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response
{
    public class CompanyViewCheckResponse
    {
        public string CompanyId { get; set; }
        public CompanyAuditState CheckState { get; set; }
        public string CompanyName { get; set; }
        public string Logo { get; set; }
        public string Code { get; set; }
        public string Info { get; set; }
        public string Remark { get; set; }
        public string Telphone { get; set; }
        public string Scale { get; set; }
        public string Url { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string Img { get; set; }
        public string Contact { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}