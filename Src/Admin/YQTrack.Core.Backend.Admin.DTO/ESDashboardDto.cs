using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.DTO.Output;

namespace YQTrack.Core.Backend.Admin.DTO
{
    public class ESDashboardDto
    {
        public int FPermissionId { get; set; }
        public string FDashboardSrc { get; set; }
        public int? FMaxDateRange { get; set; }
        public string FUsername { get; set; }
        public string FPassword { get; set; }
        public IEnumerable<ESFieldsConfigDto> FFieldsConfig { get; set; }
    }
}