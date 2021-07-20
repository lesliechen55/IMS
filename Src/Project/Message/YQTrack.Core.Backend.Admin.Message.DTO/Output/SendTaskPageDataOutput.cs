using System;

namespace YQTrack.Core.Backend.Admin.Message.DTO.Output
{
    /// <summary>
    /// 发送任务列表
    /// </summary>
    public class SendTaskPageDataOutput
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public long FTaskId { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public long FProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string FProjectName { get; set; }

        /// <summary>
        /// 发送渠道
        /// </summary>
        public int FChannel { get; set; }

        /// <summary>
        /// 模版名称
        /// </summary>
        public string FTemplateName { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string FRemarks { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public int FState { get; set; }

        /// <summary>
        /// 发送成功数
        /// </summary>
        public int FPushSucess { get; set; }

        /// <summary>
        /// 发送失败数
        /// </summary>
        public int FPushFail { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? FCreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? FUpdateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string FCreateBy { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string FUpdateBy { get; set; }
    }
}
