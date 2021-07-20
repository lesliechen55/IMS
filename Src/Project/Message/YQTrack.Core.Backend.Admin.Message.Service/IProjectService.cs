using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Message.Service
{
    public interface IProjectService : IScopeService
    {
        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <returns></returns>
        Task<List<ProjectOutput>> GetAllDataAsync();
        /// <summary>
        /// 获取所有通道
        /// </summary>
        /// <returns></returns>
        List<ChannelOutput> GetAllChannels();
    }
}
