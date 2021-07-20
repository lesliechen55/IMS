using System;

namespace YQTrack.Core.Backend.Admin.TrackApi.Data.Models
{
    public partial class TTrackQuota
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public long FUserId { set; get; }

        /// <summary>
        /// 当前有效的总额度
        /// </summary>
        public int FQuota { set; get; }

        /// <summary>
        /// 已经使用
        /// </summary>
        public int FUsed { set; get; }

        /// <summary>
        /// 剩余额度
        /// </summary>
        public int FRemain { set; get; }

        /// <summary>
        /// 当前日期
        /// </summary>
        public DateTime FToday { set; get; }

        /// <summary>
        /// 当前日期使用数量
        /// </summary>
        public int FTodayUsed { set; get; }
    }
}
