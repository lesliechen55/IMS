using System;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Output
{
    public class ReportPageDataOutput
    {
        public long FId { get; set; }
        public string FChannelTitle { get; set; }
        public string FCompanyName { get; set; }
        public short FReasonType { get; set; }
        public string FDetail { get; set; }
        public string FReportEmail { get; set; }
        public DateTime FReportTime { get; set; }
        public short FProcessStatus { get; set; }
        public DateTime? FProcessTime { get; set; }
        public string FProcessRemark { get; set; }
        public string FProcessDescription { get; set; }
    }
}