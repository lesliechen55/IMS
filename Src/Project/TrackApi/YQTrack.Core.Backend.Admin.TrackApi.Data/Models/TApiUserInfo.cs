using System;

namespace YQTrack.Core.Backend.Admin.TrackApi.Data.Models
{
    public partial class TApiUserInfo
    {
        public long FUserId { get; set; }
        public short FUserNo { get; set; }
        public string FUserName { get; set; }
        public string FEmail { get; set; }
        public string FTrackFrequency { get; set; }
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
        public string FContactPhone { get; set; }
        public string FContactEmail { get; set; }
        public string FRemark { get; set; }
        /// <summary>
        /// 结算货币
        /// </summary>
        public byte FCurrency { get; set; }
        public bool FIsChinese { get; set; }
        public byte FAuditState { get; set; }
        public byte FApiState { get; set; }
        public DateTime FCreatedTime { get; set; }
        public long FCreatedBy { get; set; }
        public DateTime FUpdateTime { get; set; }
        public long FUpdateBy { get; set; }
    }
}
