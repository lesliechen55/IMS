using System;
using System.Collections.Concurrent;
using YQTrack.Backend.Message.Model;
using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Backend.Message.Model.Models;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Configuration;
using YQTrack.Log;
using YQTrack.RabbitMQ;
using YQTrack.RabbitMQ.Model;
using YQTrack.RabbitMQ.Queue;

namespace YQTrack.Core.Backend.Admin.Message.Core.Message
{
    /// <summary>
    /// 消息发送帮助库
    /// </summary>
    public static class MessageHelper
    {
        private static readonly ConcurrentDictionary<MessageTemplateType, RabbitQueue<MessageModel>> Dic = new ConcurrentDictionary<MessageTemplateType, RabbitQueue<MessageModel>>();

        private static readonly ConcurrentDictionary<MessageTemplateType, RabbitQueue<SendCallbackModel>> DicCall = new ConcurrentDictionary<MessageTemplateType, RabbitQueue<SendCallbackModel>>();

        private static RabbitMQManager _rabbitMqManager = new RabbitMQManager(ConfigManager.Initialize<ImsRabbitConfig>());

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="model">消息模型</param>
        /// <returns></returns>
        public static bool SendMessage(MessageModel model)
        {
            if (model == null)
            {
                LogHelper.LogObj(LogDefine.LogWarn, "发送消息,消息模型是null");
                return false;
            }
            var queue = Dic.GetOrAdd(model.MessageType, key =>
            {
                RabbitQueue<MessageModel> result = null;
                var exchangeName = MessageQueueDefine.MessageExchange;
                var queueName = $"{MessageQueueDefine.MessageQueueGroup}.{key}";

                try
                {
                    result = _rabbitMqManager.CreateQueue<MessageModel>(new QueueInfo(exchangeName, queueName) { ExpireTime = TimeSpan.FromDays(7) });
                    Dic[key] = result;
                }
                catch (Exception ex)
                {
                    LogHelper.LogObj(LogDefine.LogError, ex, new { msg = $"创建消息队列异常4,exchange={exchangeName},qeueueName={queueName},expired=new TimeSpan(7, 0, 0, 0)" });
                }

                return result;
            });

            return queue.Publish(model, true);
        }


        /// <summary>
        /// 发送消息(推送与邮件回调使用)
        /// </summary>
        /// <param name="model">消息模型</param>
        /// <returns></returns>
        public static bool SendCallback(SendCallbackModel model)
        {
            if (model == null)
            {
                LogHelper.LogObj(LogDefine.LogWarn, "发送消息,消息模型是null");
                return false;
            }
            var queue = DicCall.GetOrAdd(model.TemplateType, key =>
            {
                RabbitQueue<SendCallbackModel> result = null;
                var exchangeName = MessageQueueDefine.MessagedCallbackExchange;
                var queueName = $"{MessageQueueDefine.MessagedCallbackQueueGroup}.{key}";

                try
                {
                    RabbitMQType rabbitMqType = key.GetRemark();
                    if (rabbitMqType == RabbitMQType.ALL || rabbitMqType == RabbitMQType.MessageCallBack)
                    {
                        result = _rabbitMqManager.CreateQueue<SendCallbackModel>(new QueueInfo(exchangeName, queueName) { ExpireTime = TimeSpan.FromDays(7) });
                        DicCall[key] = result;
                    }

                }
                catch (Exception ex)
                {
                    LogHelper.LogObj(LogDefine.LogError, ex, new { msg = $"创建消息队列异常2,exchange={exchangeName},qeueueName={queueName},expired=new TimeSpan(7, 0, 0, 0)" });
                }

                return result;
            });

            return queue.Publish(model, true);
        }
    }
}
