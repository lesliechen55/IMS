using YQTrack.SentinelRedis;
using YQTrack.SentinelRedis.Config;
using YQTrack.Configuration;

namespace YQTrack.Core.Backend.Admin.Core
{
    public static class SentinelRedisHelper
    {
        /// <summary>
        /// 单例：静态内部类，避免了线程不安全，延迟加载，效率高。
        /// </summary>
        public static class Default
        {
            private static RedisConfig config = ConfigManager.Initialize<TrackInfoRedisConfig>();

            private static RedisConfig config0 { get { config.Database = 0; return config.Clone(); } }
            private static RedisConfig config2 { get { config.Database = 2; return config.Clone(); } }
            public static SentinelRedisControler SentinelRedisControler0 { get; } = new SentinelRedisControler(config0);
            public static SentinelRedisControler SentinelRedisControler2 { get; } = new SentinelRedisControler(config2);
        }
    }
}
