using Newtonsoft.Json;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response
{
    public class UserConfigResponse
    {
        public long UserId { get; set; }
        public short UserNo { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// 访问密钥
        /// </summary>
        public string AppSecretKey { set; get; }
        /// <summary>
        /// WebHook地址
        /// </summary>
        public string WebHook { get; set; }

        /// <summary>
        /// IP白名单
        /// </summary>
        public ICollection<string> IPWhiteList { get; set; }

        /// <summary>
        /// 联系人名称
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// 联系人电话
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 联系人邮箱
        /// </summary>
        public string ContactEmail { get; set; }

        /// <summary>
        /// 调度频率
        /// </summary>
        public string ScheduleFrequency { get; set; }

        /// <summary>
        /// 通知配置
        /// </summary>
        public ApiNotify ApiNotify { set; get; }
    }
    public class ApiNotify
    {
        /// <summary>
        /// 推送配置
        /// </summary>
        [JsonProperty("push")]
        public ApiPushConfig ApiPushConfig { get; private set; }

        /// <summary>
        /// 剩余额度配置
        /// </summary>
        [JsonProperty("quota")]
        public QuotaConfig QuotaConfig { get; private set; }
    }
    /// <summary>
    /// 推送事件信息
    /// </summary>
    public class ApiPushConfig
    {
        /// <summary>
        /// 推送选项对应子状态
        /// </summary>
        [JsonProperty("d")]
        public ICollection<int> PushStates { get; private set; }

        /// <summary>
        /// 每日连续推送错误次数预警值
        /// </summary>
        [JsonProperty("e")]
        public int ErrorCountWarningValue { get; set; }
    }

    /// <summary>
    /// 剩余额度配置信息
    /// </summary>
    public class QuotaConfig
    {
        /// <summary>
        /// 剩余额度预警值
        /// </summary>
        [JsonProperty("w")]
        public int QuotaWarningValue { get; set; }
    }
}
