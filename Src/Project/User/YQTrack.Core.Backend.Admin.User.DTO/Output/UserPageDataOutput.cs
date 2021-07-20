using System;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.User.DTO.Output
{
    public class UserPageDataOutput
    {
        public long FuserId { get; set; }
        public UserRoleType? FuserRole { get; set; }
        public byte? FnodeId { get; set; }
        public byte? FdbNo { get; set; }
        public string FnickName { get; set; }
        public string Femail { get; set; }
        public byte Fstate { get; set; }
        public DateTime? FlastSignIn { get; set; }
        public DateTime? FcreateTime { get; set; }
        public long? FcreateBy { get; set; }
        public DateTime? FupdateTime { get; set; }
        public long? FupdateBy { get; set; }
        public string Flanguage { get; set; }
        public int? Fcountry { get; set; }
        public int? FisPay { get; set; }
        public byte? Fsource { get; set; }
    }
}