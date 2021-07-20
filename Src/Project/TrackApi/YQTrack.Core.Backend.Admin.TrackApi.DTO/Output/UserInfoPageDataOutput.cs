using System;

namespace YQTrack.Core.Backend.Admin.TrackApi.DTO.Output
{
    public class UserInfoPageDataOutput
    {
        public long FUserId { get; set; }
        public short FUserNo { get; set; }
        public string FUserName { get; set; }

        /// <summary>
        /// 剩余额度
        /// </summary>
        public int FRemain { set; get; }

        /// <summary>
        /// 当前日期使用数量
        /// </summary>
        public int FTodayUsed { set; get; }

        public string FContactName { get; set; }
        public string FContactPhone { get; set; }
        public byte FApiState { get; set; }
        public DateTime FCreatedTime { get; set; }

        public byte? FScheduleFrequency { get; set; }
        public byte? FGiftQuota { get; set; }

        public string FEmail { get; set; }
        public string FContactEmail { get; set; }
    }
    public class QuoteOutput
    {
        public long FUserId { get; set; }

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
