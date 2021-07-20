using YQTrack.Backend.RedisCache;

namespace YQTrack.Core.Backend.Admin.Core
{
    public static class IdHelper
    {
        /// <summary>
        /// 从Redis的master节点获取唯一识别Id
        /// </summary>
        /// <returns></returns>
        public static long GetGenerateId()
        {
            var generateId = RedisService.GenerateId("master", 1);
            return generateId;
        }
    }
}
