using System;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Response
{
    public class BatchTaskPageDataResponse
    {
        /// <summary>
        ///  批量操作ID
        /// </summary>
        public string BatchId { set; get; }
        /// <summary>
        ///  任务类型
        /// </summary>
        public string TaskType { set; get; }
        /// <summary>
        ///  任务开始时间
        /// </summary>
        public DateTime? TaskStartTime { set; get; }
        /// <summary>
        /// 任务结束时间
        /// </summary>
        public DateTime? TaskEndTime { set; get; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public string TaskStatus { set; get; }
        /// <summary>
        /// 处理总数
        /// </summary>
        public string Total { set; get; }
        /// <summary>
        ///  成功总数
        /// </summary>
        public string Success { set; get; }
        /// <summary>
        ///  错误总数
        /// </summary>
        public string Error { set; get; }
        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
