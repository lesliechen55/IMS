using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Input;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Output;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Service
{
    public interface IStatisticsService : IScopeService
    {
        /// <summary>
        /// 获取跟踪统计分析数据
        /// </summary>
        /// <param name="chartDateType"></param>
        /// <param name="email"></param>
        /// <param name="enable"></param>
        /// <returns></returns>
        Task<ChartOutput> GetTrackAnalysisOutputAsync(ChartDateType chartDateType, string email, bool? enable);

        /// <summary>
        /// 获取用户标签管理使用功能记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<(IEnumerable<UserMarkLogPageDataOutput> outputs, int total)> GetUserMarkLogPageDataAsync(UserMarkLogPageDataInput input);

        /// <summary>
        /// 获取用户报表数据
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<IEnumerable<ReportOutput>> GetExportAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// 获取用户标签使用数据
        /// </summary>
        /// <param name="email"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<IEnumerable<ExportUserMarkLogOutput>> GetExportUserMarkLogAsync(string email, DateTime? startDate, DateTime? endDate);
    }
}