using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.DTO.Output
{
    public class ESDashboardDetailOutput
    {
        public ESDashboardDto ESDashboard { get; set; }

        public IEnumerable<ESFieldOutput> TimeRanges { get; set; }

        public string[] Categories { get; set; }
    }
}