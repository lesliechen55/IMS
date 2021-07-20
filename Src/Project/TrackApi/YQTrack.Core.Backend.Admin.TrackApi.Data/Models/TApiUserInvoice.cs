using System;

namespace YQTrack.Core.Backend.Admin.TrackApi.Data.Models
{
    public partial class TApiUserInvoice
    {
        public int FInvoiceId { get; set; }
        public long FUserId { get; set; }
        public DateTime FIssueTime { get; set; }
        public DateTime FStartDate { get; set; }
        public DateTime FEndDate { get; set; }
        public int FTotalRequest { get; set; }
        /// <summary>
        /// 账单人民币金额
        /// </summary>
        public decimal FCNYAmount { set; get; }
        /// <summary>
        /// 账单美元金额
        /// </summary>
        public decimal FUSDAmount { set; get; }
        public string FData { get; set; }
    }
}
