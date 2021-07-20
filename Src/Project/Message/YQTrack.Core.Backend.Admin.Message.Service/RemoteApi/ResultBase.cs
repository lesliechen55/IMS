namespace YQTrack.Core.Backend.Admin.Message.Service.RemoteApi
{
    public class ResultBase
    {
        /// <summary>
        /// 结果代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 渲染返回 为空则渲染失败
        /// </summary>
        public string Json { get; set; }
    }
}