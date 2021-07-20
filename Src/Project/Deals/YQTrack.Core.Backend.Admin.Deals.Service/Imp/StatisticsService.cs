using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Deals.Core;
using YQTrack.Core.Backend.Admin.Deals.Data;
using YQTrack.Core.Backend.Admin.Deals.DTO.Input;
using YQTrack.Core.Backend.Admin.Deals.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Deals.Service.Imp
{
    public class StatisticsService : IStatisticsService
    {
        private readonly DealsDbContext _dbContext;

        public StatisticsService(DealsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取统计信息列表
        /// </summary>
        /// <param name="input">发送任务列表搜索条件</param>
        /// <returns></returns>
        public async Task<StatisticsAllListOutput> GetStatisticsListDataAsync(StatisticsServiceInput input)
        {
            var baseList = await GetStatisticsAllOutputListByDate(input);

            var result = new StatisticsAllListOutput()
            {
                ClickRate = GetStatisticsAllFromClickRate(baseList),
                TransactionRate = GetStatisticsAllFromTransactionRate(baseList),
                PaymentRate = GetStatisticsAllFromPaymentRate(baseList),
                ConversionRate = GetStatisticsAllFromConversionRate(baseList),
                ECPCRate = GetStatisticsAllFromECPCRate(baseList),
                AllRate = GetStatisticsAllFromAll(baseList)
            };

            return result;
        }

        /// <summary>
        /// 获取统计信息-按日期
        /// </summary>
        /// <param name="input">发送任务列表搜索条件</param>
        /// <returns></returns>
        public async Task<IEnumerable<StatisticsAllOutput>> GetPageDataContrastDataAsync(StatisticsServiceInput input)
        {
            var outputs = await _dbContext.TYQStatisticsAll
                .Where(t => t.FWeekNum == DateHelper.GetWeekNum(input.StartTime) || t.FWeekNum == DateHelper.GetWeekNum(input.EndTime))
                .ProjectTo<StatisticsAllOutput>()
                .OrderByDescending(x => x.FStatisticsDate)
                .ToListAsync();

            if (outputs != null && outputs.Count > 1)
            {
                outputs[0].FClickRate = DataHelper.GetGrowthRate(outputs[0].FClickCount, outputs[1].FClickCount);
                outputs[0].FTransactionRate = DataHelper.GetGrowthRate(outputs[0].FTransactionCount, outputs[1].FTransactionCount);
                outputs[0].FPaymentRate = DataHelper.GetGrowthRate(outputs[0].FPaymentCount, outputs[1].FPaymentCount);
                outputs[0].FConversionRate = DataHelper.GetGrowthRate(outputs[0].FConversion, outputs[1].FConversion);
                outputs[0].FECPCRate = DataHelper.GetGrowthRate(outputs[0].FECPC, outputs[1].FECPC);

                outputs[1].FClickRate = 100;
                outputs[1].FTransactionRate = 100;
                outputs[1].FPaymentRate = 100;
                outputs[1].FConversionRate = 100;
                outputs[1].FECPCRate = 100;
            }

            if (outputs != null && outputs.Count == 1)
            {
                outputs[0].FClickRate = 100;
                outputs[0].FTransactionRate = 100;
                outputs[0].FPaymentRate = 100;
                outputs[0].FConversionRate = 100;
                outputs[0].FECPCRate = 100;
            }

            return (outputs);
        }

        public async Task<IEnumerable<StatisticsMerOutput>> GetStatisticsMerStartDataAsync(StatisticsServiceInput input)
        {
            int weekNum = DateHelper.GetWeekNum(input.StartTime);
            var outputs = await (from s in _dbContext.TYQStatisticsMer
                                 join t in _dbContext.TYQMerchantLibraryLang
                                 on s.FYQMerchantLibraryId equals t.FYQMerchantLibraryId into temp
                                 from t in temp.DefaultIfEmpty()
                                 where t.FLangCode == "01" && s.FWeekNum == weekNum
                                 select new StatisticsMerOutput
                                 {
                                     FId = s.FId,
                                     FYQMerchantLibraryId = s.FYQMerchantLibraryId,
                                     FName = t == null ? "" : t.FName,
                                     FClickCount = s.FClickCount,
                                     FTransactionCount = s.FTransactionCount,
                                     FConversion = s.FConversion,
                                     FPaymentCount = s.FPaymentCount,
                                     FECPC = s.FECPC,
                                     FPrioritys = s.FPrioritys,
                                     FStatisticsDate = s.FStatisticsDate,
                                     FWeekNum = s.FWeekNum,
                                     FCreateDate = s.FCreateDate,
                                     FModifyDate = s.FModifyDate
                                 })
               .OrderBy(t => t.FPrioritys)
               .ToListAsync();

            return outputs;
        }

        public async Task<IEnumerable<StatisticsMerOutput>> GetStatisticsMerEndDataAsync(StatisticsServiceInput input)
        {
            int weekNum = DateHelper.GetWeekNum(input.EndTime);
            var outputs = await (from s in _dbContext.TYQStatisticsMer
                                 join t in _dbContext.TYQMerchantLibraryLang
                                 on s.FYQMerchantLibraryId equals t.FYQMerchantLibraryId into temp
                                 from t in temp.DefaultIfEmpty()
                                 where t.FLangCode == "01" && s.FWeekNum == weekNum
                                 select new StatisticsMerOutput
                                 {
                                     FId = s.FId,
                                     FYQMerchantLibraryId = s.FYQMerchantLibraryId,
                                     FName = t == null ? "" : t.FName,
                                     FClickCount = s.FClickCount,
                                     FTransactionCount = s.FTransactionCount,
                                     FConversion = s.FConversion,
                                     FPaymentCount = s.FPaymentCount,
                                     FECPC = s.FECPC,
                                     FPrioritys = s.FPrioritys,
                                     FStatisticsDate = s.FStatisticsDate,
                                     FWeekNum = s.FWeekNum,
                                     FCreateDate = s.FCreateDate,
                                     FModifyDate = s.FModifyDate
                                 })
              .OrderBy(t => t.FPrioritys)
              .ToListAsync();

            return outputs;
        }

        #region 内部逻辑

        /// <summary>
        /// 根据时间区间获取统计数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<List<StatisticsAllOutput>> GetStatisticsAllOutputListByDate(StatisticsServiceInput input)
        {
            var weekNumStart = DateHelper.GetWeekNum(input.StartTime);
            var weekNumEnd = DateHelper.GetWeekNum(input.EndTime);
            var outputs = await _dbContext.TYQStatisticsAll
                .Where(t => t.FWeekNum >= weekNumStart && t.FWeekNum <= weekNumEnd)
                .ProjectTo<StatisticsAllOutput>()
                .OrderBy(x => x.FWeekNum)
                .ToListAsync();
            return outputs;
        }


        private static ChartOutput GetStatisticsAllFromClickRate(IReadOnlyCollection<StatisticsAllOutput> baseList)
        {
            var result = new ChartOutput
            {
                Title = "点击数趋势图",
                XAxisData = baseList.Select(t => t.FStatisticsDate).ToList(),
                Series = new List<SerieItemOutput> {
                    new SerieItemOutput{
                        Name ="ClickCount",
                        Data =baseList.Select(t=>(decimal)t.FClickCount).ToList()
                    }
                }
            };
            return result;
        }

        private static ChartOutput GetStatisticsAllFromTransactionRate(IReadOnlyCollection<StatisticsAllOutput> baseList)
        {
            var result = new ChartOutput
            {
                Title = "转化数趋势图",
                XAxisData = baseList.Select(t => t.FStatisticsDate).ToList(),
                Series = new List<SerieItemOutput> {
                    new SerieItemOutput{
                        Name ="TransactionCount",
                        Data =baseList.Select(t=>(decimal)t.FTransactionCount).ToList()
                    }
                }
            };
            return result;
        }

        private static ChartOutput GetStatisticsAllFromPaymentRate(IReadOnlyCollection<StatisticsAllOutput> baseList)
        {
            var result = new ChartOutput
            {
                Title = "佣金趋势图",
                XAxisData = baseList.Select(t => t.FStatisticsDate).ToList(),
                Series = new List<SerieItemOutput> {
                    new SerieItemOutput{
                        Name ="PaymentCount",
                        Data =baseList.Select(t=>t.FPaymentCount).ToList()
                    }
                }
            };
            return result;
        }

        private static ChartOutput GetStatisticsAllFromConversionRate(IReadOnlyCollection<StatisticsAllOutput> baseList)
        {
            var result = new ChartOutput
            {
                Title = "转化率趋势图",
                XAxisData = baseList.Select(t => t.FStatisticsDate).ToList(),
                Series = new List<SerieItemOutput> {
                    new SerieItemOutput{
                        Name ="Conversion",
                        Data =baseList.Select(t=>t.FConversion).ToList()
                    }
                }
            };
            return result;
        }

        private static ChartOutput GetStatisticsAllFromECPCRate(IReadOnlyCollection<StatisticsAllOutput> baseList)
        {
            var result = new ChartOutput
            {
                Title = "ECPC趋势图",
                XAxisData = baseList.Select(t => t.FStatisticsDate).ToList(),
                Series = new List<SerieItemOutput> {
                    new SerieItemOutput{
                        Name ="ECPC",
                        Data =baseList.Select(t=>t.FECPC).ToList()
                    }
                }
            };
            return result;
        }

        private static ChartOutput GetStatisticsAllFromAll(IReadOnlyCollection<StatisticsAllOutput> baseList)
        {
            var result = new ChartOutput
            {
                Title = "总体趋势图",
                XAxisData = baseList.Select(t => t.FStatisticsDate).ToList(),
                Series = new List<SerieItemOutput> {
                    new SerieItemOutput{
                        Name ="ClickCount",
                        Data =baseList.Select(t=>(decimal)t.FClickCount).ToList()
                    },
                    new SerieItemOutput{
                        Name ="TransactionCount",
                        Data =baseList.Select(t=>(decimal)t.FTransactionCount * 10).ToList()
                    },
                    new SerieItemOutput{
                        Name ="PaymentCount",
                        Data =baseList.Select(t=>t.FPaymentCount * 10).ToList()
                    },
                    new SerieItemOutput{
                        Name ="Conversion",
                        Data =baseList.Select(t=>t.FConversion * 1000).ToList()
                    },
                    new SerieItemOutput{
                        Name ="ECPC",
                        Data =baseList.Select(t=>t.FECPC * 1000).ToList()
                    }
                }
            };
            return result;
        }

        #endregion
    }
}
