using System;
using YQTrack.Backend.BaseModel;

namespace YQTrack.Core.Backend.Admin.Message.DTO
{
    public class TemplateRenderData
    {
        /// <summary>
        /// 模板自身数据
        /// </summary>
        public string DataJson { get; set; }
        /// <summary>
        /// 其他项目传递的数据
        /// </summary>
        public object MessageData { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string TemplateContent { get; set; }
        /// <summary>
        /// 模板ID（唯一）
        /// </summary>
        public long FTemplateId { get; set; }
        /// <summary>
        /// 模板动态数据(不同模板在数据库中获取的)
        /// </summary>
        public dynamic ProducerData { get; set; }

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfoDTO UserInfo { get; set; }

        /// <summary>
        /// 模版语言
        /// </summary>
        public string Language { get; set; }
    }

}
