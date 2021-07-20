using System;
using YQTrack.Core.Backend.Admin.Message.Core.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response
{
    /// <summary>
    /// 发送任务列表
    /// </summary>
    public class SendTaskPageDataResponse
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 发送渠道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 模版名称
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 任务描述
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public PushState State { get; set; }

        /// <summary>
        /// 发送状态
        /// </summary>
        public string StateName { get; set; }

        /// <summary>
        /// 发送成功数
        /// </summary>
        public int PushSucess { get; set; }

        /// <summary>
        /// 发送失败数
        /// </summary>
        public int PushFail { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }
    }
}
