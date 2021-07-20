using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Configuration;
using YQTrack.Utility;

namespace YQTrack.Core.Backend.Admin.Core
{
    public class MongoDBHelper: ISingletonService
    {
        public MongoDBHelper()
        {
            foreach (var dbShardNo in ShardDbHelper.ShardDbNumberList)
            {
                MongoConfig config = ConfigManager.Initialize<MongoConfig>();
                MongoClient mongoClient = new MongoClient(config.Connection);
                var mongoDatabase = mongoClient.GetDatabase(config.DataBase + dbShardNo);
                IMongoCollection<BsonDocument> collection = mongoDatabase.GetCollection<BsonDocument>(config.Table);
                dicMongoCollection.AddOrUpdate(dbShardNo, collection, (key, value) => { return value = collection; });
            }
        }

        /// <summary>
        /// 存储Mongodb对象
        /// </summary>
        ConcurrentDictionary<string, IMongoCollection<BsonDocument>> dicMongoCollection = new ConcurrentDictionary<string, IMongoCollection<BsonDocument>>();

        /// <summary>
        /// 异步查找数据
        /// </summary>
        /// <param name="number">单号</param>
        /// <returns></returns>
        public async Task<BsonDocument> FindOneAsync(string number)
        {
            var client = GetMongoCollection(number);
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", number);
            return await client.FindAsync(filter, null).Result.FirstOrDefaultAsync();
        }

        #region 私有方法

        /// <summary>
        /// 根据单号获取分片连接操作对象
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private IMongoCollection<BsonDocument> GetMongoCollection(string number)
        {
            string dbNo = ShardDbHelper.GetShardDbNo(number);
            if (dicMongoCollection.Keys.Contains(dbNo))
            {
                return dicMongoCollection[dbNo];
            }
            throw new KeyNotFoundException(dbNo);
        }


        #endregion
    }

    public static class ShardDbHelper
    {
        /// <summary>
        /// 数据库分区编号
        /// </summary>
        static string[] _ShardDbNumberList = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };

        public static ICollection<string> ShardDbNumberList { get { return _ShardDbNumberList; } }

        /// <summary>
        /// 根据单号得到分片的编号
        /// </summary>
        /// <param name="number">单号</param>
        /// <returns>表名</returns>
        public static string GetShardDbNo(string number)
        {
            int dbNo = Math.Abs((SystemExtend.GetHashCode(number) % 10)) + 1;
            return dbNo.ToString().PadLeft(2, '0');
        }
    }
}
