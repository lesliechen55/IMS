using System.ComponentModel;
using YQTrack.Backend.Enums;
using YQTrack.Core.Backend.Enums.TrackApi;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request
{
    public class UserInfoEditRequest
    {
        [DisplayName("用户编号")]
        public short UserNo { get; set; }
        [DisplayName("用户名称")]
        public string UserName { get; set; }
        [DisplayName("注册邮箱")]
        public string Email { get; set; }
        //public int MaxTrackReq { get; set; }
        public int Remain { get; set; }
        //public string TrackFrequency { get; set; }
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
        /// <summary>
        /// 结算货币
        /// </summary>
        public CurrencyType Currency { get; set; }
        public bool IsChinese { get; set; }
        //public ApiState ApiState { get; set; }
        public string Remark { get; set; }

        /// <summary>
        /// 调度频率
        /// </summary>
        public ScheduleFrequency? ScheduleFrequency { get; set; }

        /// <summary>
        /// 下单赠送配额
        /// </summary>
        public byte? GiftQuota { get; set; }
    }
}
