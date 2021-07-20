namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response
{
    public class UserInfoViewResponse
    {
        public long? UserId { get; set; }
        public short UserNo { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// 当前有效的总额度
        /// </summary>
        public int Quota { set; get; }

        /// <summary>
        /// 已经使用
        /// </summary>
        public int Used { set; get; }

        /// <summary>
        /// 当前剩余（计算字段）
        /// </summary>
        public int Remain { set; get; }

        /// <summary>
        /// 当前日期使用数量
        /// </summary>
        public int TodayUsed { set; get; }

        /// <summary>
        /// 每天最大查单量（0表示无上限），其他大于0的值按照实际值进行控制
        /// </summary>
        public int MaxTrackReq { set; get; }
    }
}
