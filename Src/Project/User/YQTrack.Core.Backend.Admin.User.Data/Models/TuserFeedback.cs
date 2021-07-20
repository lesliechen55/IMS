using System;

namespace YQTrack.Core.Backend.Admin.User.Data.Models
{
    public partial class TuserFeedback
    {
        public long FfeedbackId { get; set; }
        public long FuserId { get; set; }
        public string Femail { get; set; }
        public string Fmobile { get; set; }
        public string Ftitle { get; set; }
        public string Ffeedback { get; set; }
        public DateTime? FcreateTime { get; set; }
        public long? FcreateBy { get; set; }
        public DateTime? FupdateTime { get; set; }
        public long? FupdateBy { get; set; }
        public int? Fstate { get; set; }
        public long? FreplyUserId { get; set; }
        public string FreplyContent { get; set; }
        public DateTime? FreplyTime { get; set; }
    }
}
