using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using YQTrack.Backend.RedisCache;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.Message.Core.Redis
{
    /// <summary>
    /// 缓存操作
    /// </summary>
    public class CacheHelper: ISingletonService
    {
        private static IMemoryCache _cache;

        public CacheHelper(IMemoryCache cache)
        {
            _cache = cache;
        }
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public T GetCache<T>(string cacheKey) where T : class
        {
            var objCache = GetLocalCache(cacheKey);
            if (objCache == null)
            {
                string result = RedisService.GetValue(cacheKey);
                if (!string.IsNullOrEmpty(result))
                {
                    objCache = JsonConvert.DeserializeObject<T>(result);
                    SetLocalCache(cacheKey, objCache);
                }
            }
            return objCache as T;
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <param name="objObject">值</param>
        public void SetCache(string cacheKey, object objObject)
        {
            SetCache(cacheKey, objObject, 5);
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <param name="objObject">值</param>
        /// <param name="timeout">过期时间(分钟) 本地缓存默认5分钟 Redis 48 * timeout分钟</param>
        public void SetCache(string cacheKey, object objObject, int timeout)
        {
            SetLocalCache(cacheKey, objObject, timeout);
            TimeSpan timeoutMinutes = new TimeSpan(0, timeout * 48, 0);
            RedisService.SetValue(cacheKey, objObject, timeoutMinutes);
        }


        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        public void RemoveAllCache(string cacheKey)
        {
            RedisService.DelValue(cacheKey);
            _cache.Remove(cacheKey);
        }

        /// <summary>
        /// 获取本地数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public static object GetLocalCache(string cacheKey)
        {
            var objCache = _cache.Get(cacheKey);
            return objCache;
        }


        /// <summary>
        /// 获取Redis数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <returns></returns>
        public object GetRedisCache(string cacheKey)
        {
            var objCache = RedisService.GetValue(cacheKey);
            return objCache;
        }


        /// <summary>
        /// 设置本地数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <param name="objObject">值</param>
        public void SetLocalCache(string cacheKey, object objObject)
        {
            SetLocalCache(cacheKey, objObject, 5);
        }

        /// <summary>
        /// 设置本地数据缓存
        /// </summary>
        /// <param name="cacheKey">键</param>
        /// <param name="objObject">值</param>
        /// <param name="timeout">过期时间(分钟) 本地缓存默认5分钟</param>
        public void SetLocalCache(string cacheKey, object objObject, int timeout)
        {
            if (objObject == null) return;
            _cache.Set(cacheKey, objObject, DateTime.Now.AddSeconds(timeout * 60));
        }

    }
}
