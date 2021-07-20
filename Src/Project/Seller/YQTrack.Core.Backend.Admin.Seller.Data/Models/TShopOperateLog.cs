using System;

namespace YQTrack.Core.Backend.Admin.Seller.Data.Models
{
    public partial class TShopOperateLog
    {
        public long FLogId { get; set; }
        public long FUserId { get; set; }
        public long FShopId { get; set; }
        public int FOperateType { get; set; }
        public DateTime FCreateTime { get; set; }
        public string FLogData { get; set; }
    }
}
