namespace YQTrack.Core.Backend.Enums.TrackApi
{
    /// <summary>
    /// 停止原因
    /// </summary>
    public enum StopTrackingReason
    {
        /// <summary>
        /// 未知
        /// </summary>
        None = 0,

        /// <summary>
        /// 过期自动停止
        /// </summary>
        ExpiredAutoStop = 1,

        /// <summary>
        /// 接口请求停止
        /// </summary>
        ApiRequestStop = 2,

        /// <summary>
        /// 手工操作停止
        /// </summary>
        ManualOperationStop = 3
    }
}
