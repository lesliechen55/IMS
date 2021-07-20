using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.DevOps.Service
{
    public interface ITrackInfoMongoDBService : IScopeService
    {
        /// <summary>
        /// 跟踪单号获取跟踪信息
        /// </summary>
        /// <param name="number">跟踪单号</param>
        /// <returns></returns>
        Task<(bool success, string jsonData)> GetJsonDataAsync(string number);
    }
}
