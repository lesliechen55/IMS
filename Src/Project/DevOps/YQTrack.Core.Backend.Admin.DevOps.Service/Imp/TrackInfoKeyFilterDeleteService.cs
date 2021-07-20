using StackExchange.Redis;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.DevOps.DTO;
using YQTrack.Log;
using YQTrack.SentinelRedis;

namespace YQTrack.Core.Backend.Admin.DevOps.Service.Imp
{
    public class TrackInfoKeyFilterDeleteService : ITrackInfoKeyFilterDeleteService
    {
        public TrackInfoKeyFilterDeleteService()
        {
        }
        private static readonly object lockObj = new object();
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="filter"></param>
        public void BatchDelete(string filter)
        {
            //避免重入
            lock (lockObj)
            {
                if (RedisScanDto.BatchDeleteState == BatchDeleteState.Running)
                {
                    throw new BusinessException("任务正在进行中，请稍后再试");
                }
                if (RedisScanDto.BatchDeleteState == BatchDeleteState.Cancelling)
                {
                    throw new BusinessException("任务正在取消中，请稍后再试");
                }
                RedisScanDto.BatchDeleteState = BatchDeleteState.Running;
            }
            RedisScanDto.DeleteData = new DeleteData();
            RedisScanDto.DeleteData.Filter = filter;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int totalCount = 0;
            Task.Run(() =>
            {
                string luaDeleteKeys = @"
                        return redis.call('scan',ARGV[1],'match',ARGV[2],'count',100)
                    ";
                //遍历所有缓存节点
                totalCount += LoopAllNodes(luaDeleteKeys, SentinelRedisHelper.Default.SentinelRedisControler0);
                totalCount += LoopAllNodes(luaDeleteKeys, SentinelRedisHelper.Default.SentinelRedisControler2);

            }).ContinueWith(task =>
            {
                stopwatch.Stop();
                if (task.IsFaulted)
                {
                    RedisScanDto.BatchDeleteState = BatchDeleteState.Cancelled;
                    RedisScanDto.DeleteData.Msg = $"批量删除任务出现未知异常，总共删除了{totalCount}条数据，用时{stopwatch.ElapsedMilliseconds}毫秒";
                    LogHelper.LogObj(new LogDefinition(LogLevel.Error, "BatchDeleteError"), task.Exception?.InnerException ?? task.Exception, new
                    {
                        RedisScanDto.BatchDeleteState,
                        RedisScanDto.DeleteData
                    });
                    return;
                }
                if (RedisScanDto.BatchDeleteState == BatchDeleteState.Cancelling)
                {
                    RedisScanDto.BatchDeleteState = BatchDeleteState.Cancelled;
                }
                if (RedisScanDto.BatchDeleteState == BatchDeleteState.Cancelled)
                {
                    RedisScanDto.DeleteData.Msg = $"批量删除手动取消操作成功，总共删除了{totalCount}条数据，用时{stopwatch.ElapsedMilliseconds}毫秒";
                    LogHelper.LogObj(new LogDefinition(LogLevel.Notice, "BatchDeleteCancelled"), RedisScanDto.DeleteData);
                }
                else
                {
                    RedisScanDto.BatchDeleteState = BatchDeleteState.Completed;
                    RedisScanDto.DeleteData.Msg = $"批量删除执行完毕，总共删除了{totalCount}条数据，用时{stopwatch.ElapsedMilliseconds}毫秒";
                    LogHelper.LogObj(new LogDefinition(LogLevel.Notice, "BatchDeleteCompleted"), RedisScanDto.DeleteData);
                }
            });
        }

        private static int LoopAllNodes(string luaDeleteKeys, SentinelRedisControler controler)
        {
            int count = 0;
            if (RedisScanDto.BatchDeleteState != BatchDeleteState.Running)
            {
                return 0;
            }
            foreach (string node in controler.RedisNodes)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                int nodeCount = 0;
                int cursor = 0;
                do
                {
                    RedisResult[] results = (RedisResult[])controler.ScriptEvaluateAtNode(node, luaDeleteKeys, null, new RedisValue[] { cursor, $"*{RedisScanDto.DeleteData.Filter}*" });
                    if (results != null && results.Length > 0)
                    {
                        cursor = (int)results[0];
                        RedisResult[] resultKeys = (RedisResult[])results[1];
                        foreach (string key in resultKeys)
                        {
                            if (RedisScanDto.BatchDeleteState != BatchDeleteState.Running)//手动取消
                            {
                                cursor = 0;
                                break;
                            }
                            if (controler.KeyDelete(key))
                            {
                                lock (lockObj)
                                {
                                    if (RedisScanDto.DeleteData.BatchDeleteKeys.Count >= 50)
                                    {
                                        RedisScanDto.DeleteData.BatchDeleteKeys.RemoveAt(0);
                                    }
                                    RedisScanDto.DeleteData.BatchDeleteKeys.Add(key);
                                }
                                count++;
                                nodeCount++;
                            }
                        }
                    }
                    else
                    {
                        cursor = 0;
                    }
                } while (cursor != 0);
                stopwatch.Stop();
                if (RedisScanDto.BatchDeleteState == BatchDeleteState.Cancelling)//手动取消
                {
                    LogHelper.LogObj(new LogDefinition(LogLevel.Notice, "BatchDeleteNodeCancelling"), new { msg = $"节点：{node}批量删除时手动取消操作，总共删除了{nodeCount}条数据，用时{stopwatch.ElapsedMilliseconds}毫秒" });
                    RedisScanDto.BatchDeleteState = BatchDeleteState.Cancelled;
                    break;
                }
                else
                {
                    LogHelper.LogObj(new LogDefinition(LogLevel.Notice, "BatchDeleteNodeCompleted"), new { msg = $"节点：{node}批量删除执行完毕，总共删除了{nodeCount}条数据，用时{stopwatch.ElapsedMilliseconds}毫秒" });
                }
            }
            return count;
        }

        /// <summary>
        /// 获取批量删除是否已完成
        /// </summary>
        /// <returns></returns>
        public (BatchDeleteState batchDeleteState, string filter) GetBatchDeleteState()
        {
            return (RedisScanDto.BatchDeleteState, RedisScanDto.DeleteData.Filter);
        }

        /// <summary>
        /// 获取已批量删除的缓存键
        /// </summary>
        /// <returns></returns>
        public (BatchDeleteState batchDeleteState, DeleteData deleteData) GetBatchDeleteKeys(string key)
        {
            lock (lockObj)
            {
                List<string> batchDeleteKeys = RedisScanDto.DeleteData.BatchDeleteKeys;
                int index = batchDeleteKeys.FindIndex(f => f.Equals(key, System.StringComparison.OrdinalIgnoreCase));
                if (index > -1)
                {
                    batchDeleteKeys.RemoveRange(0, index + 1);
                }
            }
            DeleteData deleteData = RedisScanDto.DeleteData;
            if (RedisScanDto.BatchDeleteState == BatchDeleteState.Completed || RedisScanDto.BatchDeleteState == BatchDeleteState.Cancelled)
            {
                RedisScanDto.DeleteData = new DeleteData();
            }
            return (RedisScanDto.BatchDeleteState, deleteData);
        }

        /// <summary>
        /// 取消批量删除
        /// </summary>
        /// <returns></returns>
        public void CancelBatchDelete()
        {
            lock (lockObj)
            {
                if (RedisScanDto.BatchDeleteState == BatchDeleteState.Running)
                    RedisScanDto.BatchDeleteState = BatchDeleteState.Cancelling;
            }
        }
    }

}
