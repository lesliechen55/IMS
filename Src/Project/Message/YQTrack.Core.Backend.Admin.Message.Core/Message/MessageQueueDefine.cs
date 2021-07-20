namespace YQTrack.Core.Backend.Admin.Message.Core.Message
{
    /// <summary>
    /// 消息中心
    /// </summary>
    public static class MessageQueueDefine
    {
        /// <summary>
        /// 消息中心接受消息交换机
        /// </summary>
        public static readonly string MessageExchange = "Message";

        /// <summary>
        /// 消息中心接受消息交换机- 队列前缀
        /// </summary>
        public static readonly string MessageQueueGroup = "Message";

        /// <summary>
        /// 消息中心回调订阅交换机
        /// </summary>
        public static readonly string MessagedCallbackExchange = "MessagedCallback";


        /// <summary>
        /// 消息中心回调订阅交换机 -队列前缀
        /// </summary>
        public static readonly string MessagedCallbackQueueGroup = "MessagedCallback";
    }
}
