using System;
using YQTrack.Core.Backend.Enums.Seller;

namespace YQTrack.Core.Backend.Admin.Seller.DTO.Output
{
    public class BatchTaskPageDataOutput
    {
        /// <summary>
        ///  批量操作ID
        /// </summary>
        public long FBatchId { set; get; }
        /// <summary>
        ///  任务类型
        /// </summary>
        public int FTaskType { set; get; }
        /// <summary>
        ///  任务开始时间
        /// </summary>
        public DateTime? FTaskStartTime { set; get; }
        /// <summary>
        /// 任务结束时间
        /// </summary>
        public DateTime? FTaskEndTime { set; get; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public TrackBatchTaskStatus FTaskStatus { set; get; }
        /// <summary>
        /// 处理总数
        /// </summary>
        public int FTotal { set; get; }
        /// <summary>
        ///  成功总数
        /// </summary>
        public int FSuccess { set; get; }
        /// <summary>
        ///  错误总数
        /// </summary>
        public int FError { set; get; }
        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime FCreateTime { get; set; }
    }
}
