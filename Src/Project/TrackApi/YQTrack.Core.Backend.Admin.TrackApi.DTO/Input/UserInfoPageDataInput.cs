using System;

namespace YQTrack.Core.Backend.Admin.TrackApi.DTO.Input
{
    public class UserInfoPageDataInput : PageInput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// API使用状态
        /// </summary>
        public byte? ApiState { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
