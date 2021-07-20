using System.ComponentModel;
using YQTrack.Backend.RedisCache;
using YQTrack.Configuration;
using YQTrack.RabbitMQ.Model;
using YQTrack.Backend.Sharding;
using YQTrack.Backend.Sharding.Config;
using RedisConfig = YQTrack.SentinelRedis.Config.RedisConfig;
using YQTrack.Backend.LanguageHelper;

namespace YQTrack.Core.Backend.Admin.Core
{

    [Category("RabbitMQ-Default")]
    public class ImsRabbitConfig : RabbitConfig { }

    [Category("RabbitMQ-RPC")]
    public class RpcRabbitConfig : RabbitConfig { }

    [Category("DBSharding-ApiTrackConfig")]
    public class DBApiTrackConfig : DBShardingConfig { }

    [Category("DBSharding-CarrierTrackConfig")]
    public class CarrierTrackDbShardingConfig : DBShardingConfig { }

    [Category("DBSharding-SellerConfig")]
    public class SellerOrderDbShardingConfig : DBShardingConfig { }

    [Category("DBSharding-SellerMessage")]
    public class SellerMessageDbShardingConfig : DBShardingConfig { }

    [Category("RedisCluster-TrackInfo")]
    public class TrackInfoRedisConfig : RedisConfig { }

    /// <summary>
    /// 采集信息存储信息
    /// </summary>
    [Category("MongoCluster-TrackInfo")]
    public class MongoConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 库
        /// </summary>
        public string DataBase { get; set; }
        /// <summary>
        /// 表
        /// </summary>
        public string Table { get; set; }
    }

    public static class SettingsHelper
    {
        /// <summary>
        /// 初始化读取设置
        /// </summary>
        public static void InitConfig()
        {
            ConfigManager.Initialize(typeof(RedisConfigDefault));
            LanguageManage.Init();
            DBShardingRouteFactory.InitDBShardingConfig(ConfigManager.Initialize<DBApiTrackConfig>());
            DBShardingRouteFactory.InitDBShardingConfig(ConfigManager.Initialize<CarrierTrackDbShardingConfig>());
            DBShardingRouteFactory.InitDBShardingConfig(ConfigManager.Initialize<SellerOrderDbShardingConfig>());
            DBShardingRouteFactory.InitDBShardingConfig(ConfigManager.Initialize<SellerMessageDbShardingConfig>());
        }
    }
}