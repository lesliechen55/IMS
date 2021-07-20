using System;
using YQTrack.Backend.Enums;
using YQTrack.Core.Backend.Enums.TrackApi;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response
{
    public class UserInfoResponse
    {
        public long? UserId { get; set; }
        public short UserNo { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        //public string TrackFrequency { get; set; }
        //public int MaxTrackReq { get; set; }
        public int Remain { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { set; get; }
        /// <summary>
        /// 增值税号
        /// </summary>
        public string VATNo { set; get; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { set; get; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { set; get; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public CurrencyType Currency { get; set; }
        public bool IsChinese { get; set; }
        public string ApiState { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Remark { get; set; }

        public ScheduleFrequency ScheduleFrequency { get; set; }
        public byte GiftQuota { get; set; }
    }
}
