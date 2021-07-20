using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ReconcilePageDataResponse
    {
        public long ReconcileId { get; set; }
        public string ProviderId { get; set; }
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 对账笔数
        /// </summary>
        public int SheetCount { get; set; }
        /// <summary>
        /// 成功笔数
        /// </summary>
        public int SuccessCount { get; set; }
        /// <summary>
        /// 失败笔数
        /// </summary>
        public int FailedCount { get; set; }
        /// <summary>
        /// 不存在笔数
        /// </summary>
        public int NotExistCount { get; set; }
        /// <summary>
        /// 总笔数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 退款笔数
        /// </summary>
        public int RefundedCount { get; set; }
    }
}
