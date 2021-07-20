using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Input
{
    public class CompanyPageDataInput : PageInput
    {
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public CompanyAuditState? Status { get; set; }
    }
}