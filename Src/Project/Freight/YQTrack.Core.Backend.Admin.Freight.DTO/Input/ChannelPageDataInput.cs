using System;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Freight.DTO.Input
{
    public class ChannelPageDataInput : PageInput
    {
        public long? Id { get; set; }

        public string Name { get; set; }

        public DateTime? PublishStartTime { get; set; }

        public DateTime? PublishEndTime { get; set; }

        public DateTime? ExpireStartTime { get; set; }

        public DateTime? ExpireEndTime { get; set; }

        public ChannelState? Status { get; set; }
    }
}