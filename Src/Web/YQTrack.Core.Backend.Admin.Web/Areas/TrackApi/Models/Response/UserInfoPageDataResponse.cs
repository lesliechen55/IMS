using System;
using YQTrack.Core.Backend.Enums.TrackApi;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response
{
    public class UserInfoPageDataResponse
    {
        public string UserId { get; set; }
        public short UserNo { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 剩余额度
        /// </summary>
        public string Remain { set; get; }

        /// <summary>
        /// 当前日期使用数量
        /// </summary>
        public string TodayUsed { set; get; }

        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public ApiState ApiState { get; set; }
        public DateTime CreatedTime { get; set; }

        public string ScheduleFrequency { get; set; }
        public string GiftQuota { get; set; }

        public string Email { get; set; }
        public string ContactEmail { get; set; }
    }
}
