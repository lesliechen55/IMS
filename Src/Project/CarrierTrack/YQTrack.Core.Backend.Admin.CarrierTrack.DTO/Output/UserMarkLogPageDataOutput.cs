using System;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Output
{
    public class UserMarkLogPageDataOutput
    {
        public long FUserId { get; set; }
        public string FEmail { get; set; }
        public string FDescription { get; set; }
        public string FDetail { get; set; }
        public DateTime FCreateTime { get; set; }
    }
}