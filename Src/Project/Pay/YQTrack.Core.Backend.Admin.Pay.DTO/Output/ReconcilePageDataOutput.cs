using System;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.DTO.Output
{
    public class ReconcilePageDataOutput
    {
        public long FReconcileId { get; set; }
        public PaymentProvider FProviderId { get; set; }
        public DateTime FBeginTime { get; set; }
        public int FSheetCount { get; set; }
        public int FSuccessCount { get; set; }
        public int FFailedCount { get; set; }
        public int FNotExistCount { get; set; }
        public int FTotalCount { get; set; }
        public int FRefundedCount { get; set; }
    }
}
