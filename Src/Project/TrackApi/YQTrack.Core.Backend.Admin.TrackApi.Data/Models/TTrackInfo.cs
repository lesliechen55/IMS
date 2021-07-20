using System;
using YQTrack.Core.Backend.Enums.TrackApi;

namespace YQTrack.Core.Backend.Admin.TrackApi.Data.Models
{
    public partial class TTrackInfo
    {
        public long FTrackInfoId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>
        public long FUserId { set; get; }
        /// <summary>
        /// 运单编号
        /// </summary>
        public string FTrackNo { set; get; }
        /// <summary>
        /// 第一运输商
        /// </summary>
        public int FFirstCarrier { set; get; }
        /// <summary>
        /// 第一运输商国家
        /// </summary>
        public int FFirstCountry { set; get; }
        /// <summary>
        /// 第二运输商
        /// </summary>
        public int FSecondCarrier { set; get; }
        /// <summary>
        /// 第二运输商国家
        /// </summary>
        public int FSecondCountry { set; get; }
        /// <summary>
        /// 包裹状态
        /// </summary>
        public int FPackageState { set; get; }

        /// <summary>
        /// 包裹状态最后变更时间
        /// </summary>
        public DateTime? FPackageStateChangeTime { get; set; }
        /// <summary>
        /// 运单注册时间
        /// </summary>
        public DateTime FRegisterTime { set; get; }
        /// <summary>
        /// 最后跟踪时间
        /// </summary>
        public DateTime FLastTrackTime { set; get; }

        /// <summary>
        /// 事件的hash值
        /// </summary>
        public int FEventsHash { get; set; }

        /// <summary>
        /// Hash变更的最后时间
        /// </summary>
        public DateTime? FEventsHashChangeTime { get; set; }
        /// <summary>
        /// 下次跟踪时间
        /// </summary>
        public DateTime FNextTrackingTime { set; get; }
        /// <summary>
        /// 最后推送时间
        /// </summary>
        public DateTime FLastPushTime { set; get; }
        /// <summary>
        /// 最后推送状态（成功/失败）
        /// </summary>
        public short FLastPushState { set; get; }
        /// <summary>
        /// 总计推送失败次数
        /// </summary>
        public int FTotalPushFailure { set; get; }
        /// <summary>
        /// 总计推送成功次数
        /// </summary>
        public int FTotalPushSuccess { set; get; }
        /// <summary>
        /// 停止跟踪开始时间，需要按照这个时间都对数据进行分区，便于后期多历史数据的切出和归档
        /// </summary>
        public DateTime FStopTrackingTime { set; get; }
        /// <summary>
        /// 停止原因
        /// </summary>
        public StopTrackingReason FStopTrackingReason { set; get; }
        /// <summary>
        /// 是否正跟跟踪中（0，停止跟踪，1：正在跟踪）
        /// </summary>
        public bool FTrackingState { set; get; }
        /// <summary>
        /// 是否已经重新跟踪过了
        /// </summary>
        public bool FIsRetracked { get; set; }
    }
}
