using YQTrack.Core.Backend.Enums.TrackApi;

namespace YQTrack.Core.Backend.Admin.TrackApi.DTO.Input
{
    public class UserInfoEditInput
    {
        public short FUserNo { get; set; }
        public string FUserName { get; set; }
        public string FEmail { get; set; }
        //public int FMaxTrackReq { get; set; }
        public int FRemain { get; set; }
        //public string FTrackFrequency { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string FCompanyName { set; get; }
        /// <summary>
        /// 增值税号
        /// </summary>
        public string FVATNo { set; get; }
        /// <summary>
        /// 地址
        /// </summary>
        public string FAddress { set; get; }
        /// <summary>
        /// 国家
        /// </summary>
        public string FCountry { set; get; }
        public string FContactName { get; set; }
        public string FContactEmail { get; set; }
        public string FContactPhone { get; set; }
        /// <summary>
        /// 结算货币
        /// </summary>
        public byte FCurrency { get; set; }
        public bool FIsChinese { get; set; }
        //public ApiState FApiState { get; set; }
        public string FRemark { get; set; }

        public ScheduleFrequency ScheduleFrequency { get; set; }
        public byte GiftQuota { get; set; }
    }
}
