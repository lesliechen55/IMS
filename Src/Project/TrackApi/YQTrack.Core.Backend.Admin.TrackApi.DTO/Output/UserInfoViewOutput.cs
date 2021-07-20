namespace YQTrack.Core.Backend.Admin.TrackApi.DTO.Output
{
    public class UserInfoViewOutput
    {
        public long? FUserId { get; set; }
        public short FUserNo { get; set; }
        public string FUserName { get; set; }
        /// <summary>
        /// 当前有效的总额度
        /// </summary>
        public int FQuota { set; get; }

        /// <summary>
        /// 已经使用
        /// </summary>
        public int FUsed { set; get; }

        /// <summary>
        /// 每天最大查单量（0表示无上限），其他大于0的值按照实际值进行控制
        /// </summary>
        public int FMaxTrackReq { set; get; }

        /// <summary>
        /// 当前剩余（计算字段）
        /// </summary>
        public int FRemain { set; get; }

        /// <summary>
        /// 当前日期使用数量
        /// </summary>
        public int FTodayUsed { set; get; }
    }
}
