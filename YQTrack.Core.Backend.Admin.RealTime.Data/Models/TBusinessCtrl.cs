using System;
using System.Collections.Generic;
using System.Text;

namespace YQTrack.Core.Backend.Admin.RealTime.Data.Models
{
    public partial class TBusinessCtrl
    {
        public long FCtrlId { get; set; }
        public long FUserId { get; set; }
        public long FPurchaseOrderId { get; set; }
        public string FProductSkuId { get; set; }
        public byte FConsumeType { get; set; }
        public short FBusinessCtrlType { get; set; }
        public int FServiceCount { get; set; }
        public bool FAvailable { get; set; }
        public byte FValidPeriod { get; set; }
        public DateTime FStartTime { get; set; }
        public DateTime? FStopTime { get; set; }
        public int? FUsedCount { get; set; }
        public int? FRemainCount { get; set; }
        public string FExtra { get; set; }
        public string FRemark { get; set; }
        public int? FProviderId { get; set; }
        public DateTime? FCreateTime { get; set; }
        public long? FCreateBy { get; set; }
        public DateTime? FUpdateTime { get; set; }
        public long? FUpdateBy { get; set; }
        public byte? FInvalidReason { get; set; }
    }
}
