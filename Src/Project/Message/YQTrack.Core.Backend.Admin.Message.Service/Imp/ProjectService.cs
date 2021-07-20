using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.Data;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Message.Service.Imp
{
    public class ProjectService : IProjectService
    {
        private readonly MessageDbContext _dbContext;

        public ProjectService(MessageDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectOutput>> GetAllDataAsync()
        {
            var outputs = _dbContext.Tproject.OrderBy(x => x.FprojectName)
                .ProjectTo<ProjectOutput>()
                .ToListAsync();

            return await outputs;
        }

        /// <summary>
        /// 获取所有通道
        /// </summary>
        /// <returns></returns>
        public List<ChannelOutput> GetAllChannels()
        {
            var output = new List<ChannelOutput>();
            foreach (ChannelSend item in Enum.GetValues(typeof(ChannelSend)))
            {
                output.Add(new ChannelOutput()
                {
                    ChannelId = (int)item,
                    ChannelName = item.GetDescription()
                });
            }
            return output;
        }
    }
}
