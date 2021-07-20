using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.TrackApi.Data;
using YQTrack.Core.Backend.Admin.TrackApi.Data.Models;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;
using YQTrack.Core.Backend.Admin.TrackApi.Service.Imp.Dto;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.TrackApi.Service.Imp
{
    public class AnalyzeService : IAnalyzeService
    {
        private readonly ApiUserDbContext _dbApiUserContext;

        public AnalyzeService(ApiUserDbContext dbApiUserContext)
        {
            _dbApiUserContext = dbApiUserContext;
        }

        /// <summary>
        /// 查询指定用户或者全局的API查询使用趋势图(相当于只有一条线的走势图:要么是指定用户要么是全局)
        /// </summary>
        /// <param name="chartDateType"></param>
        /// <param name="userNo"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ChartOutput> GetRegisterAnalysisOutputAsync(ChartDateType chartDateType, int? userNo, string userName)
        {
            var (startTime, endTime) = ChartDateTimeHelper.InitTime(chartDateType);

            var chartOutput = new ChartOutput
            {
                Title = "API全局使用情况",
                Series = new List<SerieItemOutput>()
                {
                    new SerieItemOutput()
                    {
                        Name = "全局",
                        Data = new List<decimal>()
                    }
                }
            };

            TApiUserInfo apiUserInfo = null;
            if (userNo.HasValue || userName.IsNotNullOrWhiteSpace())
            {
                var userInfos = await _dbApiUserContext.TApiUserInfo
                    .WhereIf(() => userNo.HasValue && userNo.Value > 0, x => x.FUserNo == userNo.Value)
                    .WhereIf(userName.IsNotNullOrWhiteSpace, x => x.FUserName == userName.Trim())
                    .AsNoTracking()
                    .ToListAsync();
                if (userInfos == null || !userInfos.Any())
                {
                    throw new BusinessException($"根据搜索条件找不到任何用户数据,请检查重试");
                }
                if (userInfos.Count > 1)
                {
                    throw new BusinessException($"根据搜索条件匹配到多个用户数据,请检查重试");
                }
                apiUserInfo = userInfos.First();
            }

            if (apiUserInfo != null)
            {
                chartOutput.Title = $"用户 {apiUserInfo.FUserName}({apiUserInfo.FEmail}) 使用情况";
                chartOutput.Series[0].Name = apiUserInfo.FUserName;
            }

            List<AnalyzeDto> dtos;
            using (var connection =
                new SqlConnection(_dbApiUserContext.Database.GetDbConnection().ConnectionString))
            {
                string groupDateKeySql;
                switch (chartDateType)
                {
                    case ChartDateType.Day:
                        groupDateKeySql = @"convert(char(10), a.FDate, 120)";
                        break;
                    case ChartDateType.Week:
                        groupDateKeySql = @"datepart(year,a.FDate),datepart(WEEK,a.FDate)";
                        break;
                    case ChartDateType.Month:
                        groupDateKeySql = @"convert(char(7),a.FDate,120)";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(chartDateType), chartDateType, null);
                }

                var userIdSql = apiUserInfo != null ? $@"and a.FUserId={apiUserInfo.FUserId}" : string.Empty;

                var cmd = $@"
                    select
                    {(chartDateType == ChartDateType.Week ? "CONCAT(datepart(year,a.FDate),'第',datepart(WEEK,a.FDate),'周')" : groupDateKeySql)} as DateFormat,
                    isnull(sum(a.FCount),0) as Total
                    from dbo.TApiTrackCount as a
                    where a.FDate >= @startTime and a.FDate < @endTime {userIdSql}
                    group by {groupDateKeySql}
                    order by {groupDateKeySql} asc
                ";

                dtos = (await connection.QueryAsync<AnalyzeDto>(new CommandDefinition(cmd, new
                {
                    startTime,
                    endTime
                }))).ToList();
            }

            ProcessChartOutput(chartDateType, startTime, endTime, chartOutput, dtos);

            return chartOutput;
        }

        public async Task<IEnumerable<AutoCompleteOutput>> GetAutoCompleteOutputAsync(string userName)
        {
            if (userName.IsNullOrWhiteSpace()) return new List<AutoCompleteOutput>();
            var outputs = await _dbApiUserContext.TApiUserInfo.Where(x => x.FUserName.Contains(userName.Trim())).OrderBy(x => x.FUserName).AsNoTracking().ProjectTo<AutoCompleteOutput>().ToListAsync();
            return outputs;
        }

        private static void ProcessChartOutput(ChartDateType chartDateType, DateTime startTime,
            DateTime endTime, ChartOutput output, IReadOnlyCollection<AnalyzeDto> dtos)
        {
            switch (chartDateType)
            {
                case ChartDateType.Day:
                    for (var date = startTime; date.Date < endTime; date = date.AddDays(1))
                    {
                        var dateStr = date.ToString("yyyy-MM-dd");
                        LoopProcessChartOutput(dateStr, output, dtos);
                    }
                    break;
                case ChartDateType.Week:
                    var weekFormatList = ChartDateTimeHelper.GetWeekFormatList(startTime, endTime);
                    foreach (var week in weekFormatList)
                    {
                        LoopProcessChartOutput(week, output, dtos);
                    }
                    break;
                case ChartDateType.Month:
                    for (var date = startTime; date.Date < endTime; date = date.AddMonths(1))
                    {
                        var dateStr = date.ToString("yyyy-MM");
                        LoopProcessChartOutput(dateStr, output, dtos);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(chartDateType), chartDateType, null);
            }
        }

        private static void LoopProcessChartOutput(string dateStr, ChartOutput output,
            IReadOnlyCollection<AnalyzeDto> dtos)
        {
            output.XAxisData.Add(dateStr);

            // 判断是否当前日期有数据存在
            if (dtos.Any(x => x.DateFormat == dateStr))
            {
                var analyzeDto = dtos.Single(x => x.DateFormat == dateStr);
                output.Series.ForEach(x =>
                {
                    x.Data.Add(analyzeDto.Total);
                });
            }
            else
            {
                // 初始化所有情况为0
                output.Series.ForEach(x => x.Data.Add(0));
            }
        }

    }
}