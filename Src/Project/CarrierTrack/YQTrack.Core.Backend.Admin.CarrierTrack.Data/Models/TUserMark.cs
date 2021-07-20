using System;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models
{
    public partial class TUserMark
    {
        public long FMarkId { get; set; }
        public long FUserId { get; set; }
        public string FMarkName { get; set; }
        public int? FSortNo { get; set; }
        public DateTime FCreateTime { get; set; }
        public long FCreateBy { get; set; }
        public DateTime? FUpdateTime { get; set; }
        public long? FUpdateBy { get; set; }
        public byte? FColorFlag { get; set; }
    }
}
