using System;
using System.Collections.Generic;
using System.Text;

namespace YQTrack.Core.Backend.Admin.RealTime.Data.Models
{
    public partial class TTrackInfo
    {
        public long FTrackInfoId { get; set; }
        public long FUserId { get; set; }
        public string FTrackNo { get; set; }
        public long FShopId { get; set; } // by austin
        public int FArchivedState { get; set; } // by austin

        public int FExcelCarrier { get; set; }
        public string FExcelCountry { get; set; }
        public string FExcelProduct { get; set; }
        public string FExcelPostCode { get; set; }
        public string FExcelCustomerCode { get; set; }
        public bool FHasMark { get; set; }
        public int? FPackageState { get; set; }
        public int FTrackStateType { get; set; }
        public int FFirstCarrier { get; set; }
        public short? FFirstCountry { get; set; }
        public int? FSecondCarrier { get; set; }
        public short? FSecondCountry { get; set; }
        public string FLastEvent { get; set; }
        public bool FIsCompleted { get; set; }
        public DateTime? FCompletedTime { get; set; }
        public DateTime? FLastEventUpdate { get; set; }
        public DateTime? FFirstScheduleTime { get; set; }
        public DateTime? FNextScheduleTime { get; set; }
        public DateTime FCreateTime { get; set; }
        public long FCreateBy { get; set; }
        public DateTime? FUpdateTime { get; set; }
        public long? FUpdateBy { get; set; }
    }
}
