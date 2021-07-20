using System;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models
{
    public partial class TTrackUploadRecord
    {
        public long FUploadRecordId { get; set; }
        public long FUserId { get; set; }
        public string FSourceFileName { get; set; }
        public string FCurrentFileName { get; set; }
        public string FFilePath { get; set; }
        public int FRowTotal { get; set; }
        public int? FSuccessTotal { get; set; }
        public int? FSuccessInsertTotal { get; set; }
        public int? FSuccessUpdateTotal { get; set; }
        public int? FErrorTotal { get; set; }
        public string FErrorDetail { get; set; }
        public string FFileMD5 { get; set; }
        public DateTime FCreateTime { get; set; }
        public long FCreateBy { get; set; }
        public DateTime? FUpdateTime { get; set; }
        public long? FUpdateBy { get; set; }
    }
}
