using System;

namespace YQTrack.Core.Backend.Admin.TrackApi.DTO.Output
{
    public class UserInfoOutput
    {
        public long? FUserId { get; set; }
        public short FUserNo { get; set; }
        public string FUserName { get; set; }
        public string FEmail { get; set; }
        //public string FTrackFrequency { get; set; }
        //public int FMaxTrackReq { get; set; }
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
        public bool FIsChinese { get; set; }
        public byte FCurrency { get; set; }
        public byte FApiState { get; set; }
        public DateTime FCreatedTime { get; set; }
        public string FRemark { get; set; }

        public byte FScheduleFrequency { get; set; }
        public byte FGiftQuota { get; set; }
    }
}
