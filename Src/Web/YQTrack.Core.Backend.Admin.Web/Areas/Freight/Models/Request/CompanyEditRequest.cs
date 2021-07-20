namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request
{
    public class CompanyEditRequest
    {
        public long Id { get; set; }
        public short Limit { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }
        public string Phone { get; set; }
    }
}