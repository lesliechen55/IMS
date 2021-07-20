using System;

namespace YQTrack.Core.Backend.Admin.User.DTO.Output
{
    public class UserFeedbackPageDataOutput
    {
        public long FfeedbackId { get; set; }
        public long FuserId { get; set; }
        public string Femail { get; set; }
        public string Ffeedback { get; set; }
        public int? Fstate { get; set; }
        public string FreplyContent { get; set; }
        public DateTime? FcreateTime { get; set; }
        public DateTime? FreplyTime { get; set; }
        public long? FreplyUserId { get; set; }
    }
}