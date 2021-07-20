using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Log.Data;
using YQTrack.Core.Backend.Admin.Log.DTO.Output;
using YQTrack.Core.Backend.Admin.Log.Service.Imp.Dto;
using YQTrack.Core.Backend.Enums.Pay;
using YQTrack.Core.Backend.Enums.User;

namespace YQTrack.Core.Backend.Admin.Log.Service.Imp
{
    public class AnalysisService : IAnalysisService
    {
        private readonly LogDbContext _logDbContext;

        public AnalysisService(LogDbContext logDbContext)
        {
            _logDbContext = logDbContext;
        }

        private static List<SerieItemOutput> InitSeriesByAnalysisType(AnalysisType analysisType)
        {
            var list = new List<SerieItemOutput>();
            switch (analysisType)
            {
                case AnalysisType.WebUserRegisterByDay:
                case AnalysisType.AppUserRegisterByDay:
                    list.AddRange(new List<UserRoleType>
                    {
                        UserRoleType.None,UserRoleType.Seller,UserRoleType.Buyer,UserRoleType.Carrier
                    }.Select(x => new SerieItemOutput
                    {
                        Name = x.GetDescription(),
                        Data = new List<decimal>()
                    }).ToList());
                    break;

                case AnalysisType.UserPositiveByDay:
                    list.AddRange(new List<UserRoleType>
                    {
                        UserRoleType.Seller,UserRoleType.Buyer,UserRoleType.Carrier,UserRoleType.Guest
                    }.Select(x => new SerieItemOutput
                    {
                        Name = x.GetDescription(),
                        Data = new List<decimal>()
                    }).ToList());
                    break;

                case AnalysisType.PlatformAccountByDay:
                    list.AddRange(EnumHelper.GetDescriptionList<PlatformType>().Select(x => new SerieItemOutput
                    {
                        Name = x,
                        Data = new List<decimal>()
                    }).ToList());
                    break;

                case AnalysisType.AppUserPositiveByDay:
                    list.AddRange(EnumHelper.GetDescriptionList<DeviceType>().Select(x => new SerieItemOutput
                    {
                        Name = x,
                        Data = new List<decimal>()
                    }).ToList());
                    break;
            }

            return list;
        }

        private async Task<IEnumerable<AnalysisDbQueryDto>> GetAnalysisDbQueryDataAsync(AnalysisType analysisType, ChartDateType chartDateType, DateTime startTime, DateTime endTime)
        {
            List<AnalysisDbQueryDto> dtos;
            using (var connection = new SqlConnection(_logDbContext.Database.GetDbConnection().ConnectionString))
            {
                var groupDateKeySql = string.Empty;
                switch (chartDateType)
                {
                    case ChartDateType.Day:
                        groupDateKeySql = $@"convert(char(10), a.FStatisticDate, 120)";
                        break;
                    case ChartDateType.Week:
                        groupDateKeySql = $@"datepart(year,a.FStatisticDate),datepart(WEEK,a.FStatisticDate)";
                        break;
                    case ChartDateType.Month:
                        groupDateKeySql = $@"convert(char(7),a.FStatisticDate,120)";
                        break;
                }

                var cmd = $@"
                select 
                    {(chartDateType == ChartDateType.Week ? "CONCAT(datepart(year,a.FStatisticDate),'第',datepart(WEEK,a.FStatisticDate),'周')" : groupDateKeySql)} as DateFormat,

                    sum(case a.FType when {(int)UserRoleType.None} then a.FCount else 0 end) as NoneCount,
                    sum(case a.FType when {(int)UserRoleType.Seller} then a.FCount else 0 end) as SellerCount,
                    sum(case a.FType when {(int)UserRoleType.Buyer} then a.FCount else 0 end) as BuyerCount,
                    sum(case a.FType when {(int)UserRoleType.Carrier} then a.FCount else 0 end) as CarrierCount,
                    sum(case a.FType when {(int)UserRoleType.Guest} then a.FCount else 0 end) as GuestCount,
                 
                    sum(case a.FType when {(int)PlatformType.Aliexpress} then a.FCount else 0 end) as AliexpressCount,
                    sum(case a.FType when {(int)PlatformType.Ebay} then a.FCount else 0 end) as EbayCount,
                    sum(case a.FType when {(int)PlatformType.DHgate} then a.FCount else 0 end) as DHgateCount,
                    sum(case a.FType when {(int)PlatformType.Wish} then a.FCount else 0 end) as WishCount,
                    
                    sum(case a.FType when {(int)DeviceType.None} then a.FCount else 0 end) as NoneDeviceTypeCount,
                    sum(case a.FType when {(int)DeviceType.IPhone} then a.FCount else 0 end) as IPhoneCount,
                    sum(case a.FType when {(int)DeviceType.IPad} then a.FCount else 0 end) as IPadCount,
                    sum(case a.FType when {(int)DeviceType.Android} then a.FCount else 0 end) as AndroidCount,
                    sum(case a.FType when {(int)DeviceType.AndroidPad} then a.FCount else 0 end) as AndroidPadCount,
                    sum(case a.FType when {(int)DeviceType.WPPhone} then a.FCount else 0 end) as WPPhoneCount,
                    sum(case a.FType when {(int)DeviceType.WPPC} then a.FCount else 0 end) as WPPCCount

                from dbo.TStatisticDay as a
                where a.FStatisticType={(int)analysisType} and
                      a.FStatisticDate >= @startTime and a.FStatisticDate < @endTime
                group by {groupDateKeySql}
                order by {groupDateKeySql} asc
                ";
                dtos = (await connection.QueryAsync<AnalysisDbQueryDto>(new CommandDefinition(cmd, new
                {
                    startTime,
                    endTime
                }))).ToList();
            }
            return dtos;
        }

        private static void ProcessAnalysisOutput(ChartDateType chartDateType, DateTime startTime, DateTime endTime, ChartOutput output, IEnumerable<AnalysisDbQueryDto> dtos)
        {
            switch (chartDateType)
            {
                case ChartDateType.Day:
                    for (var date = startTime; date.Date < endTime; date = date.AddDays(1))
                    {
                        var dateStr = date.ToString("yyyy-MM-dd");
                        LoopProcessAnalysisOutput(dateStr, output, dtos);
                    }
                    break;
                case ChartDateType.Week:
                    var weekFormatList = ChartDateTimeHelper.GetWeekFormatList(startTime, endTime);
                    foreach (var week in weekFormatList)
                    {
                        LoopProcessAnalysisOutput(week, output, dtos);
                    }
                    break;
                case ChartDateType.Month:
                    for (var date = startTime; date.Date < endTime; date = date.AddMonths(1))
                    {
                        var dateStr = date.ToString("yyyy-MM");
                        LoopProcessAnalysisOutput(dateStr, output, dtos);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(chartDateType), chartDateType, null);
            }
        }

        private static void LoopProcessAnalysisOutput(string dateStr, ChartOutput output, IEnumerable<AnalysisDbQueryDto> dtos)
        {
            output.XAxisData.Add(dateStr);
            var item = dtos.FirstOrDefault(x => x.DateFormat == dateStr);
            if (item != null)
            {
                output.Series.ForEach(x =>
                {
                    if (x.Name == UserRoleType.None.GetDescription())
                    {
                        x.Data.Add(item.NoneCount);
                    }
                    else if (x.Name == UserRoleType.Seller.GetDescription())
                    {
                        x.Data.Add(item.SellerCount);
                    }
                    else if (x.Name == UserRoleType.Buyer.GetDescription())
                    {
                        x.Data.Add(item.BuyerCount);
                    }
                    else if (x.Name == UserRoleType.Carrier.GetDescription())
                    {
                        x.Data.Add(item.CarrierCount);
                    }
                    else if (x.Name == UserRoleType.Guest.GetDescription())
                    {
                        x.Data.Add(item.GuestCount);
                    }
                    else if (x.Name == PlatformType.Aliexpress.GetDescription())
                    {
                        x.Data.Add(item.AliexpressCount);
                    }
                    else if (x.Name == PlatformType.DHgate.GetDescription())
                    {
                        x.Data.Add(item.DHgateCount);
                    }
                    else if (x.Name == PlatformType.Ebay.GetDescription())
                    {
                        x.Data.Add(item.EbayCount);
                    }
                    else if (x.Name == PlatformType.Wish.GetDescription())
                    {
                        x.Data.Add(item.WishCount);
                    }
                    else if (x.Name == DeviceType.Android.GetDescription())
                    {
                        x.Data.Add(item.AndroidCount);
                    }
                    else if (x.Name == DeviceType.AndroidPad.GetDescription())
                    {
                        x.Data.Add(item.AndroidPadCount);
                    }
                    else if (x.Name == DeviceType.IPad.GetDescription())
                    {
                        x.Data.Add(item.IPadCount);
                    }
                    else if (x.Name == DeviceType.IPhone.GetDescription())
                    {
                        x.Data.Add(item.IPhoneCount);
                    }
                    else if (x.Name == DeviceType.None.GetDescription())
                    {
                        x.Data.Add(item.NoneDeviceTypeCount);
                    }
                    else if (x.Name == DeviceType.WPPC.GetDescription())
                    {
                        x.Data.Add(item.WPPCCount);
                    }
                    else if (x.Name == DeviceType.WPPhone.GetDescription())
                    {
                        x.Data.Add(item.WPPhoneCount);
                    }
                });
            }
            else
            {
                output.Series.ForEach(x => x.Data.Add(0));
            }
        }

        public async Task<ChartOutput> GetAnalysisDataAsync(AnalysisType analysisType, ChartDateType chartDateType)
        {
            var output = new ChartOutput
            {
                Title = analysisType.GetDescription(),
                Series = InitSeriesByAnalysisType(analysisType)
            };

            var (startTime, endTime) = ChartDateTimeHelper.InitTime(chartDateType);

            var dtos = await GetAnalysisDbQueryDataAsync(analysisType, chartDateType, startTime, endTime);

            ProcessAnalysisOutput(chartDateType, startTime, endTime, output, dtos);

            return output;
        }


    }
}