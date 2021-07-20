using System;

namespace YQTrack.Core.Backend.Admin.User.Data.Models
{
    public partial class TuserProfile
    {
        public long FuserId { get; set; }
        public string Flanguage { get; set; }
        public string Favator { get; set; }
        public DateTime FcreateTime { get; set; }
        public long FcreateBy { get; set; }
        public DateTime FupdateTime { get; set; }
        public long FupdateBy { get; set; }
        public byte[] FtimeStamp { get; set; }
        public int? Fphoto { get; set; }
        public int? Fcountry { get; set; }
        public int? FisPay { get; set; }
        public byte? Fsource { get; set; }
        public string FmodifyEmailTimeData { get; set; }
        public string FroleCharacterData { get; set; }
    }
}
