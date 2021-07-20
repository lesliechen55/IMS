using System;
using YQTrack.Core.Backend.Enums.TrackApi;

namespace YQTrack.Core.Backend.Admin.TrackApi.DTO.Output
{
    public class UserConfigOutput
    {
        public long? FUserId { get; set; }
        public short FUserNo { get; set; }
        public string FUserName { get; set; }
        public string FEmail { get; set; }
        /// <summary>
        /// 访问密钥
        /// </summary>
        public DateTime FSecretSeed { get; set; }

        /// <summary>
        /// WebHook地址
        /// </summary>
        public string FWebHook { get; set; }

        /// <summary>
        /// IP白名单
        /// </summary>
        public string FIPWhiteList { get; set; }

        /// <summary>
        /// 联系人名称
        /// </summary>
        public string FContactName { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string FContactPhone { get; set; }

        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string FContactEmail { get; set; }

        /// <summary>
        /// 调度频率
        /// </summary>
        public ScheduleFrequency FScheduleFrequency { get; set; }

        /// <summary>
        /// 推送配置
        /// </summary>
        public string FApiNotify { set; get; }
    }
}
