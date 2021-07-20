using MongoDB.Bson;
using MongoDB.Bson.IO;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Log;

namespace YQTrack.Core.Backend.Admin.DevOps.Service.Imp
{
    public class TrackInfoMongoDBService : ITrackInfoMongoDBService
    {
        private readonly MongoDBHelper _mongoDBHelper;
        public TrackInfoMongoDBService(MongoDBHelper mongoDBHelper)
        {
            _mongoDBHelper = mongoDBHelper;
        }

        /// <summary>
        /// 跟踪单号获取跟踪信息
        /// </summary>
        /// <param name="number">跟踪单号</param>
        /// <returns></returns>
        public async Task<(bool success, string jsonData)> GetJsonDataAsync(string number)
        {
            try
            {
                var doc = await _mongoDBHelper.FindOneAsync(number);
                if (doc == null || doc.IsBsonNull || doc.ElementCount <= 0)
                {
                    return (false, "单号数据不存在");
                }
                var jsonWriterSettings = new JsonWriterSettings { OutputMode = JsonOutputMode.Strict }; // key part 
                return (true, doc.ToJson(jsonWriterSettings));
            }
            catch (System.Exception ex)
            {
                LogHelper.LogObj(new LogDefinition(LogLevel.Error, "GetMongoDBJsonData"), ex, new { number });
                return (false, ex.Message);
            }
        }
    }
}
