namespace YQTrack.Core.Backend.Admin.Message.DTO.Input
{
    /// <summary>
    /// 测试发送任务
    /// </summary>
    public class SendTemplateTestInput
    {
        /// <summary>
        /// 模版Id
        /// </summary>
        public long TemplateId { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// 对象详情(一对一)
        /// </summary>
        public string ObjDetails { get; set; }
    }
    
}
