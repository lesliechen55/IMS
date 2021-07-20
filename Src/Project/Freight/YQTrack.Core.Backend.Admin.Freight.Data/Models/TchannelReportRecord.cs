using System;

namespace YQTrack.Core.Backend.Admin.Freight.Data.Models
{
    public partial class TchannelReportRecord
    {
        public long Fid { get; set; }
        public long FreportUserId { get; set; }
        public byte FreportUserRole { get; set; }
        public string FreportUserLanguage { get; set; }
        public string FreportUserNickName { get; set; }
        public long FcompanyId { get; set; }
        public long FchannelId { get; set; }
        public short FreasonType { get; set; }
        public string Fdetail { get; set; }
        public string FreportEmail { get; set; }
        public DateTime FreportTime { get; set; }
        public short FprocessStatus { get; set; }
        public DateTime? FprocessTime { get; set; }
        public string FprocessRemark { get; set; }
        public string FprocessDescription { get; set; }
        public bool? FisJobProcessed { get; set; }
        public DateTime? FcreateTime { get; set; }
        public long? FcreateBy { get; set; }
        public DateTime? FupdateTime { get; set; }
        public long? FupdateBy { get; set; }
    }
}
