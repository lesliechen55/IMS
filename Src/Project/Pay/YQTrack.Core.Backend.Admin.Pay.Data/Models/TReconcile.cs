using System;
using System.Collections.Generic;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.Data.Models
{
    public partial class TReconcile
    {
        public TReconcile()
        {
            TReconcileItem = new HashSet<TReconcileItem>();
        }

        public long FReconcileId { get; set; }
        public PaymentProvider FProviderId { get; set; }
        public DateTime FBeginTime { get; set; }
        public DateTime? FEndTime { get; set; }
        public int FSheetCount { get; set; }
        public int FSuccessCount { get; set; }
        public int FFailedCount { get; set; }
        public int FNotExistCount { get; set; }
        public int FTotalCount { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FCreateAt { get; set; }
        public long FUpdateBy { get; set; }
        public DateTime FUpdateAt { get; set; }
        public int FRefundedCount { get; set; }

        public virtual ICollection<TReconcileItem> TReconcileItem { get; set; }
    }
}
