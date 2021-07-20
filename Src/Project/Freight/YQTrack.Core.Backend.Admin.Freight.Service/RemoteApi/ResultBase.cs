namespace YQTrack.Core.Backend.Admin.Freight.Service.RemoteApi
{
    public class ResultBase
    {
        /// <summary>
        /// 结果代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 默认code的资源消息
        /// </summary>
        public string Message { get; set; }
    }
}