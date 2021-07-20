using System;

namespace YQTrack.Core.Backend.Admin.TrackApi.Data.Models
{
    public partial class TApiUserConfig
    {
        public long FUserId { get; set; }
        public int FMaxTrackReq { get; set; }
        public string FWebHook { get; set; }
        public string FIPWhiteList { get; set; }
        public DateTime? FSecretSeed { get; set; }
        public DateTime FCreatedTime { get; set; }
        public long FCreatedBy { get; set; }
        public DateTime FUpdateTime { get; set; }
        public long FUpdateBy { get; set; }
        public byte FScheduleFrequency { get; set; }
        public byte FGiftQuota { get; set; }
        public string FApiNotify { get; set; }
    }
}
