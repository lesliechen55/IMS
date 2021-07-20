using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Pay.Service.Imp.Dto;
using YQTrack.Core.Backend.Admin.User.Data;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class ChartService : IChartService
    {
        private readonly PayDbContext _payDbContext;
        private readonly UserDbContext _userDbContext;

        public ChartService(PayDbContext payDbContext, UserDbContext userDbContext)
        {
            _payDbContext = payDbContext;
            _userDbContext = userDbContext;
        }

        #region 共有私有方法

        /// <summary>
        /// 获取订单源数据
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userId"></param>
        /// <param name="fieldOrDefaultSumValue"></param>
        /// <returns></returns>
        private async Task<List<OrderDbQueryDto>> GetOrderDataAsync(OrderChartInput input, DateTime startTime, DateTime endTime, long? userId, string fieldOrDefaultSumValue = "1")
        {
            List<OrderDbQueryDto> orders;
            string groupDateKeySql;
            switch (input.ChartDateType)
            {
                case ChartDateType.Day:
                    groupDateKeySql = $@"convert(char(10), a.FCreateAt, 120)";
                    break;
                case ChartDateType.Week:
                    groupDateKeySql = $@"datepart(year,a.FCreateAt),datepart(WEEK,a.FCreateAt)";
                    break;
                case ChartDateType.Month:
                    groupDateKeySql = $@"convert(char(7),a.FCreateAt,120)";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input.ChartDateType), input.ChartDateType, null);
            }

            string whereSql = string.Empty;
            if (userId.HasValue)
            {
                whereSql += $" and a.FUserId={userId.Value}";
            }
            string fieldSql = string.Empty;
            //默认统计总量
            if (!input.OrderAnalysisType.HasValue)
            {
                fieldSql = $",sum({fieldOrDefaultSumValue}) as TotalCount";
            }
            if (input.CurrencyType.HasValue)
            {
                if (input.OrderAnalysisType == OrderAnalysisType.CurrencyType)
                {
                    fieldSql += $@",sum(case a.FCurrencyType when {(int)input.CurrencyType.Value} then {fieldOrDefaultSumValue} else 0 end) as {(input.CurrencyType == CurrencyType.None ? "NoneCurrencyTypeCount" : $"{input.CurrencyType}Count")}";
                }
                whereSql += $" and a.FCurrencyType={(int)input.CurrencyType}";
            }
            else
            {
                if (input.OrderAnalysisType == OrderAnalysisType.CurrencyType)
                {
                    fieldSql += $@",sum(case a.FCurrencyType when {(int)CurrencyType.None} then {fieldOrDefaultSumValue} else 0 end) as NoneCurrencyTypeCount,
                    sum(case a.FCurrencyType when { (int)CurrencyType.CNY} then { fieldOrDefaultSumValue} else 0 end) as CnyCount,
                    sum(case a.FCurrencyType when { (int)CurrencyType.USD} then { fieldOrDefaultSumValue} else 0 end) as UsdCount";
                }
            }
            if (input.ServiceType.Length > 0)
            {
                string tempSql = string.Empty;
                foreach (var item in input.ServiceType)
                {
                    if (input.OrderAnalysisType == OrderAnalysisType.ServiceType)
                    {
                        fieldSql += $@",sum(case a.FServiceType when {(int)item} then {fieldOrDefaultSumValue} else 0 end) as {(item == ServiceType.Unknown ? "UnknownServiceTypeCount" : $"{item}Count")}";
                    }
                    tempSql += $"a.FServiceType={(int)item} or ";
                }
                whereSql += $" and ({tempSql.Substring(0, tempSql.Length - 4)})";
            }
            else
            {
                if (input.OrderAnalysisType == OrderAnalysisType.ServiceType)
                {
                    fieldSql += $@",sum(case a.FServiceType when {(int)ServiceType.Unknown} then {fieldOrDefaultSumValue} else 0 end) as UnknownServiceTypeCount,
                    sum(case a.FServiceType when { (int)ServiceType.Donate} then { fieldOrDefaultSumValue} else 0 end) as DonateCount,
                    sum(case a.FServiceType when { (int)ServiceType.Buyer} then { fieldOrDefaultSumValue} else 0 end) as BuyerCount,
                    sum(case a.FServiceType when { (int)ServiceType.Seller} then { fieldOrDefaultSumValue} else 0 end) as SellerCount,
                    sum(case a.FServiceType when { (int)ServiceType.Api} then { fieldOrDefaultSumValue} else 0 end) as ApiCount,
                    sum(case a.FServiceType when { (int)ServiceType.Carrier} then { fieldOrDefaultSumValue} else 0 end) as CarrierCount";
                }
            }
            if (input.PlatformType.Length > 0)
            {
                string tempSql = string.Empty;
                foreach (var item in input.PlatformType)
                {
                    if (input.OrderAnalysisType == OrderAnalysisType.PlatformType)
                    {
                        fieldSql += $@",sum(case a.FUserPlatformType when {(int)item} then {fieldOrDefaultSumValue} else 0 end) as {(item == UserPlatformType.UNKNOWN ? "UnknownPlatformTypeCount" : $"{item}Count")}";
                    }
                    tempSql += $"a.FUserPlatformType={(int)item} or ";
                }
                whereSql += $" and ({tempSql.Substring(0, tempSql.Length - 4)})";
            }
            else
            {
                if (input.OrderAnalysisType == OrderAnalysisType.PlatformType)
                {
                    fieldSql += $@",sum(case a.FUserPlatformType when {(int)UserPlatformType.UNKNOWN} then {fieldOrDefaultSumValue} else 0 end) as UnknownPlatformTypeCount,
                    sum(case a.FUserPlatformType when {(int)UserPlatformType.WEB} then {fieldOrDefaultSumValue} else 0 end) as WebCount,
                    sum(case a.FUserPlatformType when {(int)UserPlatformType.ANDROID} then {fieldOrDefaultSumValue} else 0 end) as AndroidCount,
                    sum(case a.FUserPlatformType when {(int)UserPlatformType.IOS} then {fieldOrDefaultSumValue} else 0 end) as IosCount,
                    sum(case a.FUserPlatformType when {(int)UserPlatformType.Mobile} then {fieldOrDefaultSumValue} else 0 end) as MobileCount,
                    sum(case a.FUserPlatformType when {(int)UserPlatformType.Alipay} then {fieldOrDefaultSumValue} else 0 end) as AlipayCount,
                    sum(case a.FUserPlatformType when {(int)UserPlatformType.Weixin} then {fieldOrDefaultSumValue} else 0 end) as WeixinCount";
                }
            }
            if (input.OrderStatus.Length > 0)
            {
                string tempSql = string.Empty;
                foreach (var item in input.OrderStatus)
                {
                    if (input.OrderAnalysisType == OrderAnalysisType.OrderStatus)
                    {
                        fieldSql += $@",sum(case a.FStatus when {(int)item} then {fieldOrDefaultSumValue} else 0 end) as {(item == PurchaseOrderStatus.Unknown ? "UnknownStatusCount" : $"{item}Count")}";
                    }
                    tempSql += $"a.FStatus={(int)item} or ";
                }
                whereSql += $" and ({tempSql.Substring(0, tempSql.Length - 4)})";
            }
            else
            {
                if (input.OrderAnalysisType == OrderAnalysisType.OrderStatus)
                {
                    fieldSql += $@",sum(case a.FStatus when {(int)PurchaseOrderStatus.Unknown} then {fieldOrDefaultSumValue} else 0 end) as UnknownStatusCount,
                    sum(case a.FStatus when {(int)PurchaseOrderStatus.Pending} then {fieldOrDefaultSumValue} else 0 end) as PendingCount,
                    sum(case a.FStatus when {(int)PurchaseOrderStatus.Success} then {fieldOrDefaultSumValue} else 0 end) as SuccessCount,
                    sum(case a.FStatus when {(int)PurchaseOrderStatus.Delivering} then {fieldOrDefaultSumValue} else 0 end) as DeliveringCount,
                    sum(case a.FStatus when {(int)PurchaseOrderStatus.Delivered} then {fieldOrDefaultSumValue} else 0 end) as DeliveredCount,
                    sum(case a.FStatus when {(int)PurchaseOrderStatus.Expired} then {fieldOrDefaultSumValue} else 0 end) as ExpiredCount,
                    sum(case a.FStatus when {(int)PurchaseOrderStatus.Closed} then {fieldOrDefaultSumValue} else 0 end) as ClosedCount,
                    sum(case a.FStatus when {(int)PurchaseOrderStatus.Refunding} then {fieldOrDefaultSumValue} else 0 end) as RefundingCount,
                    sum(case a.FStatus when {(int)PurchaseOrderStatus.Refunded} then {fieldOrDefaultSumValue} else 0 end) as RefundedCount,
                    sum(case a.FStatus when {(int)PurchaseOrderStatus.RefundFailure} then {fieldOrDefaultSumValue} else 0 end) as RefundFailureCount";
                }
            }
            var cmd = $@"
                select 
                    {(input.ChartDateType == ChartDateType.Week ? "CONCAT(datepart(year,a.FCreateAt),'第',datepart(WEEK,a.FCreateAt),'周')" : groupDateKeySql)} as DateFormat{fieldSql}
                from dbo.TPurchaseOrder as a
                where a.FCreateAt >= @startTime and a.FCreateAt < @endTime{whereSql}
                group by {groupDateKeySql}
                order by {groupDateKeySql} asc
                ";
            using (var connection = new SqlConnection(_payDbContext.Database.GetDbConnection().ConnectionString))
            {
                orders = (await connection.QueryAsync<OrderDbQueryDto>(new CommandDefinition(cmd, new
                {
                    startTime,
                    endTime
                }))).ToList();
            }
            return orders;
        }

        /// <summary>
        /// 获取交易源数据
        /// </summary>
        /// <param name="input"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="userId"></param>
        /// <param name="fieldOrDefaultSumValue"></param>
        /// <returns></returns>
        private async Task<List<PaymentDbQueryDto>> GetPaymentDataAsync(PaymentChartInput input, DateTime startTime, DateTime endTime, long? userId, string fieldOrDefaultSumValue = "1")
        {
            List<PaymentDbQueryDto> payments;
            string groupDateKeySql;
            switch (input.ChartDateType)
            {
                case ChartDateType.Day:
                    groupDateKeySql = $@"convert(char(10), a.FCreateAt, 120)";
                    break;
                case ChartDateType.Week:
                    groupDateKeySql = $@"datepart(year,a.FCreateAt),datepart(WEEK,a.FCreateAt)";
                    break;
                case ChartDateType.Month:
                    groupDateKeySql = $@"convert(char(7),a.FCreateAt,120)";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input.ChartDateType), input.ChartDateType, null);
            }
            string whereSql = string.Empty;
            if (userId.HasValue)
            {
                whereSql += $" and a.FPayerId={userId.Value}";
            }
            string fieldSql = string.Empty;
            //默认统计总量
            if (!input.PaymentAnalysisType.HasValue)
            {
                fieldSql = $",sum({fieldOrDefaultSumValue}) as TotalCount";
            }
            if (input.CurrencyType.HasValue)
            {
                if (input.PaymentAnalysisType == PaymentAnalysisType.CurrencyType)
                {
                    fieldSql += $@",sum(case a.FCurrencyType when {(int)input.CurrencyType.Value} then {fieldOrDefaultSumValue} else 0 end) as {(input.CurrencyType == CurrencyType.None ? "NoneCurrencyTypeCount" : $"{input.CurrencyType}Count")}";
                }
                whereSql += $" and a.FCurrencyType={(int)input.CurrencyType}";
            }
            else
            {
                if (input.PaymentAnalysisType == PaymentAnalysisType.CurrencyType)
                {
                    fieldSql += $@",sum(case a.FCurrencyType when {(int)CurrencyType.None} then {fieldOrDefaultSumValue} else 0 end) as NoneCurrencyTypeCount,
                    sum(case a.FCurrencyType when { (int)CurrencyType.CNY} then { fieldOrDefaultSumValue} else 0 end) as CnyCount,
                    sum(case a.FCurrencyType when { (int)CurrencyType.USD} then { fieldOrDefaultSumValue} else 0 end) as UsdCount";
                }
            }
            if (input.PaymentProvider.Length > 0)
            {
                string tempSql = string.Empty;
                foreach (var item in input.PaymentProvider)
                {
                    if (input.PaymentAnalysisType == PaymentAnalysisType.PaymentProvider)
                    {
                        fieldSql += $@",sum(case a.FProviderId when {(int)item} then {fieldOrDefaultSumValue} else 0 end) as {(item == PaymentProvider.Unknown ? "UnknownProviderCount" : $"{item}Count")}";
                    }
                    tempSql += $"a.FProviderId={(int)item} or ";
                }
                whereSql += $" and ({tempSql.Substring(0, tempSql.Length - 4)})";
            }
            else
            {
                if (input.PaymentAnalysisType == PaymentAnalysisType.PaymentProvider)
                {
                    fieldSql += $@",sum(case a.FProviderId when {(int)PaymentProvider.Unknown} then {fieldOrDefaultSumValue} else 0 end) as UnknownProviderCount,
	                sum(case a.FProviderId when {(int)PaymentProvider.Alipay} then {fieldOrDefaultSumValue} else 0 end) as AlipayCount,
	                sum(case a.FProviderId when {(int)PaymentProvider.Weixinpay} then {fieldOrDefaultSumValue} else 0 end) as WeixinpayCount,
	                sum(case a.FProviderId when {(int)PaymentProvider.Paypal} then {fieldOrDefaultSumValue} else 0 end) as PaypalCount,
	                sum(case a.FProviderId when {(int)PaymentProvider.Appleiap} then {fieldOrDefaultSumValue} else 0 end) as AppleiapCount,
	                sum(case a.FProviderId when {(int)PaymentProvider.Googlepay} then {fieldOrDefaultSumValue} else 0 end) as GooglepayCount,
	                sum(case a.FProviderId when {(int)PaymentProvider.Glocash} then {fieldOrDefaultSumValue} else 0 end) as GlocashCount,
	                sum(case a.FProviderId when {(int)PaymentProvider.Stripe} then {fieldOrDefaultSumValue} else 0 end) as StripeCount,
                    sum(case a.FProviderId when {(int)PaymentProvider.Shopify} then {fieldOrDefaultSumValue} else 0 end) as ShopifyCount,
	                sum(case a.FProviderId when {(int)PaymentProvider.Present} then {fieldOrDefaultSumValue} else 0 end) as PresentCount,
	                sum(case a.FProviderId when {(int)PaymentProvider.OfflinePay} then {fieldOrDefaultSumValue} else 0 end) as OfflinePayCount,
                    sum(case a.FProviderId when {(int)PaymentProvider.AlipayMiniProgram} then {fieldOrDefaultSumValue} else 0 end) as AlipayMiniPayCount,
                    sum(case a.FProviderId when {(int)PaymentProvider.WeixinpayMiniProgram} then {fieldOrDefaultSumValue} else 0 end) as WeixinMiniPayCount";
                }
            }
            if (input.ServiceType.Length > 0)
            {
                string tempSql = string.Empty;
                foreach (var item in input.ServiceType)
                {
                    if (input.PaymentAnalysisType == PaymentAnalysisType.ServiceType)
                    {
                        fieldSql += $@",sum(case a.FServiceType when {(int)item} then {fieldOrDefaultSumValue} else 0 end) as {(item == ServiceType.Unknown ? "UnknownServiceTypeCount" : $"{item}Count")}";
                    }
                    tempSql += $"a.FServiceType={(int)item} or ";
                }
                whereSql += $" and ({tempSql.Substring(0, tempSql.Length - 4)})";
            }
            else
            {
                if (input.PaymentAnalysisType == PaymentAnalysisType.ServiceType)
                {
                    fieldSql += $@",sum(case a.FServiceType when {(int)ServiceType.Unknown} then {fieldOrDefaultSumValue} else 0 end) as UnknownServiceTypeCount,
                    sum(case a.FServiceType when { (int)ServiceType.Donate} then { fieldOrDefaultSumValue} else 0 end) as DonateCount,
                    sum(case a.FServiceType when { (int)ServiceType.Buyer} then { fieldOrDefaultSumValue} else 0 end) as BuyerCount,
                    sum(case a.FServiceType when { (int)ServiceType.Seller} then { fieldOrDefaultSumValue} else 0 end) as SellerCount,
                    sum(case a.FServiceType when { (int)ServiceType.Api} then { fieldOrDefaultSumValue} else 0 end) as ApiCount,
                    sum(case a.FServiceType when { (int)ServiceType.Carrier} then { fieldOrDefaultSumValue} else 0 end) as CarrierCount";
                }
            }
            if (input.PlatformType.Length > 0)
            {
                string tempSql = string.Empty;
                foreach (var item in input.PlatformType)
                {
                    if (input.PaymentAnalysisType == PaymentAnalysisType.PlatformType)
                    {
                        fieldSql += $@",sum(case a.FPlatformType when {(int)item} then {fieldOrDefaultSumValue} else 0 end) as {(item == UserPlatformType.UNKNOWN ? "UnknownPlatformTypeCount" : $"{item}Count")}";
                    }
                    tempSql += $"a.FPlatformType={(int)item} or ";
                }
                whereSql += $" and ({tempSql.Substring(0, tempSql.Length - 4)})";
            }
            else
            {
                if (input.PaymentAnalysisType == PaymentAnalysisType.PlatformType)
                {
                    fieldSql += $@",sum(case a.FPlatformType when {(int)UserPlatformType.UNKNOWN} then {fieldOrDefaultSumValue} else 0 end) as UnknownPlatformTypeCount,
                    sum(case a.FPlatformType when {(int)UserPlatformType.WEB} then {fieldOrDefaultSumValue} else 0 end) as WebCount,
                    sum(case a.FPlatformType when {(int)UserPlatformType.ANDROID} then {fieldOrDefaultSumValue} else 0 end) as AndroidCount,
                    sum(case a.FPlatformType when {(int)UserPlatformType.IOS} then {fieldOrDefaultSumValue} else 0 end) as IosCount,
                    sum(case a.FPlatformType when {(int)UserPlatformType.Mobile} then {fieldOrDefaultSumValue} else 0 end) as MobileCount,
                    sum(case a.FPlatformType when {(int)UserPlatformType.Alipay} then {fieldOrDefaultSumValue} else 0 end) as AlipayPlatCount,
                    sum(case a.FPlatformType when {(int)UserPlatformType.Weixin} then {fieldOrDefaultSumValue} else 0 end) as WeixinCount";
                }
            }
            if (input.PaymentStatus.Length > 0)
            {
                string tempSql = string.Empty;
                foreach (var item in input.PaymentStatus)
                {
                    if (input.PaymentAnalysisType == PaymentAnalysisType.PaymentStatus)
                    {
                        fieldSql += $@",sum(case a.FPaymentStatus when {(int)item} then {fieldOrDefaultSumValue} else 0 end) as {(item == PaymentStatus.Unknown ? "UnknownStatusCount" : $"{item}Count")}";
                    }
                    tempSql += $"a.FPaymentStatus={(int)item} or ";
                }
                whereSql += $" and ({tempSql.Substring(0, tempSql.Length - 4)})";
            }
            else
            {
                if (input.PaymentAnalysisType == PaymentAnalysisType.PaymentStatus)
                {
                    fieldSql += $@",sum(case a.FPaymentStatus when {(int)PaymentStatus.Unknown} then {fieldOrDefaultSumValue} else 0 end) as UnknownStatusCount,
                    sum(case a.FPaymentStatus when {(int)PaymentStatus.Pending} then {fieldOrDefaultSumValue} else 0 end) as PendingCount,
                    sum(case a.FPaymentStatus when {(int)PaymentStatus.Success} then {fieldOrDefaultSumValue} else 0 end) as SuccessCount,
                    sum(case a.FPaymentStatus when {(int)PaymentStatus.Failed} then {fieldOrDefaultSumValue} else 0 end) as FailedCount,
                    sum(case a.FPaymentStatus when {(int)PaymentStatus.Cancelled} then {fieldOrDefaultSumValue} else 0 end) as CancelledCount,
                    sum(case a.FPaymentStatus when {(int)PaymentStatus.Refunded} then {fieldOrDefaultSumValue} else 0 end) as RefundedCount";
                }
            }
            var cmd = $@"
                select 
                    {(input.ChartDateType == ChartDateType.Week ? "CONCAT(datepart(year,a.FCreateAt),'第',datepart(WEEK,a.FCreateAt),'周')" : groupDateKeySql)} as DateFormat,
                    count(a.FPaymentId) as TotalCount{fieldSql}
                from dbo.TPayment as a
                where a.FCreateAt >= @startTime and a.FCreateAt < @endTime{whereSql}
                group by {groupDateKeySql}
                order by {groupDateKeySql} asc
                ";
            using (var connection = new SqlConnection(_payDbContext.Database.GetDbConnection().ConnectionString))
            {
                payments = (await connection.QueryAsync<PaymentDbQueryDto>(new CommandDefinition(cmd, new
                {
                    startTime,
                    endTime
                }))).ToList();
            }
            return payments;
        }

        /// <summary>
        /// 初始化订单结果数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static ChartOutput InitOrderAnalysisOutput(OrderChartInput input)
        {
            ChartOutput output = null;
            if (input.OrderAnalysisType.HasValue)
            {
                switch (input.OrderAnalysisType)
                {
                    case OrderAnalysisType.ServiceType:
                        output = new ChartOutput
                        {
                            Title = "业务类型"
                        };
                        if (input.ServiceType.Length > 0)
                        {
                            foreach (var item in input.ServiceType)
                            {
                                output.Series.Add(new SerieItemOutput
                                {
                                    Name = item.GetDescription(),
                                    Data = new List<decimal>()
                                });
                            }
                        }
                        else
                        {
                            output.Series = EnumHelper.GetDescriptionList<ServiceType>()
                            .Select(x => new SerieItemOutput
                            {
                                Name = x,
                                Data = new List<decimal>()
                            }).ToList();
                        }
                        break;
                    case OrderAnalysisType.CurrencyType:
                        output = new ChartOutput
                        {
                            Title = "货币类型",
                            Series = EnumHelper.GetDescriptionList<CurrencyType>(true)
                        .WhereIf(() => input.CurrencyType.HasValue, w => w == input.CurrencyType.GetDisplayName())
                        .Select(x => new SerieItemOutput
                        {
                            Name = x,
                            Data = new List<decimal>()
                        }).ToList()
                        };
                        break;
                    case OrderAnalysisType.PlatformType:
                        output = new ChartOutput
                        {
                            Title = "平台类型"
                        };
                        if (input.PlatformType.Length > 0)
                        {
                            foreach (var item in input.PlatformType)
                            {
                                output.Series.Add(new SerieItemOutput
                                {
                                    Name = item.GetDisplayName(),
                                    Data = new List<decimal>()
                                });
                            }
                        }
                        else
                        {
                            output.Series = EnumHelper.GetDescriptionList<UserPlatformType>(true)
                            .Select(x => new SerieItemOutput
                            {
                                Name = x,
                                Data = new List<decimal>()
                            }).ToList();
                        }
                        break;
                    case OrderAnalysisType.OrderStatus:
                        output = new ChartOutput
                        {
                            Title = "订单状态"
                        };
                        if (input.OrderStatus.Length > 0)
                        {
                            foreach (var item in input.OrderStatus)
                            {
                                output.Series.Add(new SerieItemOutput
                                {
                                    Name = item.GetDisplayName(),
                                    Data = new List<decimal>()
                                });
                            }
                        }
                        else
                        {
                            output.Series = EnumHelper.GetDescriptionList<PurchaseOrderStatus>(true)
                            .Select(x => new SerieItemOutput
                            {
                                Name = x,
                                Data = new List<decimal>()
                            }).ToList();
                        }
                        break;
                }
            }
            else
            {
                output = new ChartOutput
                {
                    Title = "订单统计",
                    Series = new List<SerieItemOutput>()
                };
                output.Series.Add(new SerieItemOutput
                {
                    Name = "总量",
                    Data = new List<decimal>()
                });
            }
            return output;
        }

        /// <summary>
        /// 初始化交易结果数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static ChartOutput InitPaymentAnalysisOutput(PaymentChartInput input)
        {
            ChartOutput output = null;
            if (input.PaymentAnalysisType.HasValue)
            {
                switch (input.PaymentAnalysisType.Value)
                {
                    case PaymentAnalysisType.ServiceType:
                        output = new ChartOutput
                        {
                            Title = "业务类型"
                        };
                        if (input.ServiceType.Length > 0)
                        {
                            foreach (var item in input.ServiceType)
                            {
                                output.Series.Add(new SerieItemOutput
                                {
                                    Name = item.GetDescription(),
                                    Data = new List<decimal>()
                                });
                            }
                        }
                        else
                        {
                            output.Series = EnumHelper.GetDescriptionList<ServiceType>()
                            .Select(x => new SerieItemOutput
                            {
                                Name = x,
                                Data = new List<decimal>()
                            }).ToList();
                        }
                        break;
                    case PaymentAnalysisType.PlatformType:
                        output = new ChartOutput
                        {
                            Title = "平台类型"
                        };
                        if (input.PlatformType.Length > 0)
                        {
                            foreach (var item in input.PlatformType)
                            {
                                output.Series.Add(new SerieItemOutput
                                {
                                    Name = item.GetDisplayName(),
                                    Data = new List<decimal>()
                                });
                            }
                        }
                        else
                        {
                            output.Series = EnumHelper.GetDescriptionList<UserPlatformType>(true)
                            .Select(x => new SerieItemOutput
                            {
                                Name = x,
                                Data = new List<decimal>()
                            }).ToList();
                        }
                        break;
                    case PaymentAnalysisType.PaymentProvider:
                        output = new ChartOutput
                        {
                            Title = "支付渠道"
                        };
                        if (input.PaymentProvider.Length > 0)
                        {
                            foreach (var item in input.PaymentProvider)
                            {
                                output.Series.Add(new SerieItemOutput
                                {
                                    Name = item.GetDescription(),
                                    Data = new List<decimal>()
                                });
                            }
                        }
                        else
                        {
                            output.Series = EnumHelper.GetDescriptionList<PaymentProvider>()
                            .Where(w => w != PaymentProvider.Escrow.GetDescription() && w != PaymentProvider.Wechatpay.GetDescription())
                            .Select(x => new SerieItemOutput
                            {
                                Name = x,
                                Data = new List<decimal>()
                            }).ToList();
                        }
                        break;
                    case PaymentAnalysisType.CurrencyType:
                        output = new ChartOutput
                        {
                            Title = "货币类型",
                            Series = EnumHelper.GetDescriptionList<CurrencyType>(true)
                        .WhereIf(() => input.CurrencyType.HasValue, w => w == input.CurrencyType.GetDisplayName())
                        .Select(x => new SerieItemOutput
                        {
                            Name = x,
                            Data = new List<decimal>()
                        }).ToList()
                        };
                        break;
                    case PaymentAnalysisType.PaymentStatus:
                        output = new ChartOutput
                        {
                            Title = "支付状态"
                        };
                        if (input.PaymentStatus.Length > 0)
                        {
                            foreach (var item in input.PaymentStatus)
                            {
                                output.Series.Add(new SerieItemOutput
                                {
                                    Name = item.GetDisplayName(),
                                    Data = new List<decimal>()
                                });
                            }
                        }
                        else
                        {
                            output.Series = EnumHelper.GetDescriptionList<PaymentAllStatus>(true)
                            .Select(x => new SerieItemOutput
                            {
                                Name = x,
                                Data = new List<decimal>()
                            }).ToList();
                        }
                        break;
                }
            }
            else
            {
                output = new ChartOutput
                {
                    Title = "交易统计",
                    Series = new List<SerieItemOutput>()
                };
                output.Series.Add(new SerieItemOutput
                {
                    Name = "总量",
                    Data = new List<decimal>()
                });
            }
            return output;
        }

        /// <summary>
        /// 处理加工订单分析数据
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="orders"></param>
        private static void LoopProcessOrderAnalysisOutput(string dateStr, OrderChartInput input, ChartOutput output, IEnumerable<OrderDbQueryDto> orders)
        {
            output.XAxisData.Add(dateStr);
            var item = orders.FirstOrDefault(x => x.DateFormat == dateStr);
            if (item != null)
            {
                if (input.OrderAnalysisType.HasValue)
                {
                    switch (input.OrderAnalysisType.Value)
                    {
                        case OrderAnalysisType.ServiceType:
                            output.Series.ForEach(x =>
                            {
                                if (x.Name == ServiceType.Unknown.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.UnknownServiceTypeCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == ServiceType.Donate.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.DonateCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == ServiceType.Buyer.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.BuyerCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == ServiceType.Seller.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.SellerCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == ServiceType.Api.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.ApiCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == ServiceType.Carrier.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.CarrierCount, 2, MidpointRounding.AwayFromZero));
                                }
                            });
                            break;
                        case OrderAnalysisType.PlatformType:
                            output.Series.ForEach(x =>
                            {
                                if (x.Name == UserPlatformType.UNKNOWN.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.UnknownPlatformTypeCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.ANDROID.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.AndroidCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.IOS.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.IosCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.WEB.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.WebCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.Mobile.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.MobileCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.Alipay.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.AlipayCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.Weixin.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.WeixinCount, 2, MidpointRounding.AwayFromZero));
                                }
                            });
                            break;
                        case OrderAnalysisType.CurrencyType:
                            output.Series.ForEach(x =>
                            {
                                if (x.Name == CurrencyType.None.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.NoneCurrencyTypeCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == CurrencyType.CNY.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.CnyCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == CurrencyType.USD.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.UsdCount, 2, MidpointRounding.AwayFromZero));
                                }
                            });
                            break;
                        case OrderAnalysisType.OrderStatus:
                            output.Series.ForEach(x =>
                            {
                                if (x.Name == PurchaseOrderStatus.Unknown.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.UnknownStatusCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PurchaseOrderStatus.Closed.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.ClosedCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PurchaseOrderStatus.Delivered.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.DeliveredCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PurchaseOrderStatus.Delivering.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.DeliveringCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PurchaseOrderStatus.Expired.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.ExpiredCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PurchaseOrderStatus.Pending.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.PendingCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PurchaseOrderStatus.Refunded.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.RefundedCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PurchaseOrderStatus.RefundFailure.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.RefundFailureCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PurchaseOrderStatus.Refunding.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.RefundingCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PurchaseOrderStatus.Success.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.SuccessCount, 2, MidpointRounding.AwayFromZero));
                                }
                            });
                            break;
                    }
                }
                else
                {
                    output.Series.ForEach(x =>
                    {
                        x.Data.Add(Math.Round(item.TotalCount, 2, MidpointRounding.AwayFromZero));
                    });
                }
            }
            else
            {
                output.Series.ForEach(x => x.Data.Add(0));
            }
        }

        /// <summary>
        /// 处理加工交易分析数据
        /// </summary>
        /// <param name="dateStr"></param>
        /// <param name="input"></param>
        /// <param name="output"></param>
        /// <param name="payments"></param>
        private static void LoopProcessPaymentAnalysisOutput(string dateStr, PaymentChartInput input, ChartOutput output, IEnumerable<PaymentDbQueryDto> payments)
        {
            output.XAxisData.Add(dateStr);
            var item = payments.FirstOrDefault(x => x.DateFormat == dateStr);
            if (item != null)
            {
                if (input.PaymentAnalysisType.HasValue)
                {
                    switch (input.PaymentAnalysisType.Value)
                    {
                        case PaymentAnalysisType.ServiceType:
                            output.Series.ForEach(x =>
                            {
                                if (x.Name == ServiceType.Unknown.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.UnknownServiceTypeCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == ServiceType.Donate.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.DonateCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == ServiceType.Buyer.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.BuyerCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == ServiceType.Seller.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.SellerCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == ServiceType.Api.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.ApiCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == ServiceType.Carrier.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.CarrierCount, 2, MidpointRounding.AwayFromZero));
                                }
                            });
                            break;
                        case PaymentAnalysisType.PlatformType:
                            output.Series.ForEach(x =>
                            {
                                if (x.Name == UserPlatformType.UNKNOWN.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.UnknownPlatformTypeCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.ANDROID.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.AndroidCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.IOS.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.IosCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.WEB.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.WebCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.Mobile.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.MobileCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.Alipay.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.AlipayPlatCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == UserPlatformType.Weixin.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.WeixinCount, 2, MidpointRounding.AwayFromZero));
                                }
                            });
                            break;
                        case PaymentAnalysisType.PaymentProvider:
                            output.Series.ForEach(x =>
                            {
                                if (x.Name == PaymentProvider.Unknown.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.UnknownProviderCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.Alipay.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.AlipayCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.Appleiap.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.AppleiapCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.Glocash.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.GlocashCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.Stripe.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.StripeCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.Shopify.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.ShopifyCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.Googlepay.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.GooglepayCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.Paypal.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.PaypalCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.Present.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.PresentCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.Weixinpay.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.WeixinpayCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.OfflinePay.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.OfflinePayCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.WeixinpayMiniProgram.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.WeixinMiniPayCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentProvider.AlipayMiniProgram.GetDescription())
                                {
                                    x.Data.Add(Math.Round(item.AlipayMiniPayCount, 2, MidpointRounding.AwayFromZero));
                                }
                            });
                            break;
                        case PaymentAnalysisType.CurrencyType:
                            output.Series.ForEach(x =>
                            {
                                if (x.Name == CurrencyType.None.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.NoneCurrencyTypeCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == CurrencyType.CNY.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.CnyCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == CurrencyType.USD.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.UsdCount, 2, MidpointRounding.AwayFromZero));
                                }
                            });
                            break;
                        case PaymentAnalysisType.PaymentStatus:
                            output.Series.ForEach(x =>
                            {
                                if (x.Name == PaymentAllStatus.Unknown.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.UnknownStatusCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentAllStatus.Pending.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.PendingCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentAllStatus.Success.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.SuccessCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentAllStatus.Failed.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.FailedCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentAllStatus.Cancelled.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.CancelledCount, 2, MidpointRounding.AwayFromZero));
                                }
                                else if (x.Name == PaymentAllStatus.Refunded.GetDisplayName())
                                {
                                    x.Data.Add(Math.Round(item.RefundedCount, 2, MidpointRounding.AwayFromZero));
                                }
                            });
                            break;
                    }
                }
                else
                {
                    output.Series.ForEach(x =>
                    {
                        x.Data.Add(Math.Round(item.TotalCount, 2, MidpointRounding.AwayFromZero));
                    });
                }
            }
            else
            {
                output.Series.ForEach(x => x.Data.Add(0));
            }
        }

        private static void ProcessOrderAnalysisOutput(OrderChartInput input, DateTime startTime,
            DateTime endTime, ChartOutput output, IReadOnlyCollection<OrderDbQueryDto> orders)
        {
            switch (input.ChartDateType)
            {
                case ChartDateType.Day:
                    for (var date = startTime; date.Date < endTime; date = date.AddDays(1))
                    {
                        var dateStr = date.ToString("yyyy-MM-dd");
                        LoopProcessOrderAnalysisOutput(dateStr, input, output, orders);
                    }
                    break;
                case ChartDateType.Week:
                    var weekFormatList = ChartDateTimeHelper.GetWeekFormatList(startTime, endTime);
                    foreach (var week in weekFormatList)
                    {
                        LoopProcessOrderAnalysisOutput(week, input, output, orders);
                    }
                    break;
                case ChartDateType.Month:
                    for (var date = startTime; date.Date < endTime; date = date.AddMonths(1))
                    {
                        var dateStr = date.ToString("yyyy-MM");
                        LoopProcessOrderAnalysisOutput(dateStr, input, output, orders);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input.ChartDateType), input.ChartDateType, null);
            }
        }

        private static void ProcessPaymentAnalysisOutPut(PaymentChartInput input, DateTime startTime,
            DateTime endTime, ChartOutput output, IReadOnlyCollection<PaymentDbQueryDto> payments)
        {
            switch (input.ChartDateType)
            {
                case ChartDateType.Day:
                    for (var date = startTime; date.Date < endTime; date = date.AddDays(1))
                    {
                        var dateStr = date.ToString("yyyy-MM-dd");
                        LoopProcessPaymentAnalysisOutput(dateStr, input, output, payments);
                    }
                    break;
                case ChartDateType.Week:
                    var weekFormatList = ChartDateTimeHelper.GetWeekFormatList(startTime, endTime);
                    foreach (var week in weekFormatList)
                    {
                        LoopProcessPaymentAnalysisOutput(week, input, output, payments);
                    }
                    break;
                case ChartDateType.Month:
                    for (var date = startTime; date.Date < endTime; date = date.AddMonths(1))
                    {
                        var dateStr = date.ToString("yyyy-MM");
                        LoopProcessPaymentAnalysisOutput(dateStr, input, output, payments);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(input.ChartDateType), input.ChartDateType, null);
            }
        }

        private async Task<long?> GetUserIdByEmailAsync(string email)
        {
            long? userId = null;
            if (email.IsNotNullOrWhiteSpace())
            {
                var tuserInfo = await _userDbContext.TuserInfo.SingleOrDefaultAsync(x => x.Femail == email.Trim());
                if (tuserInfo != null)
                {
                    userId = tuserInfo.FuserId;
                }
                else
                {
                    throw new BusinessException($"参数{nameof(email)}值错误,找不到对应用户");
                }
            }
            return userId;
        }

        #endregion

        #region 公开方法

        /// <summary>
        /// 获取订单数量分析(维度：业务类型/货币类型/平台类型/订单状态)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ChartOutput> GetOrderCountAnalysisOutputAsync(OrderChartInput input)
        {
            var userId = await GetUserIdByEmailAsync(input.Email);
            var output = InitOrderAnalysisOutput(input);
            var (startTime, endTime) = ChartDateTimeHelper.InitTime(input.ChartDateType);
            var orders = await GetOrderDataAsync(input, startTime, endTime, userId);
            ProcessOrderAnalysisOutput(input, startTime, endTime, output, orders);
            return output;
        }

        /// <summary>
        /// 订单金额分析(维度：业务类型/货币类型/平台类型/订单状态)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ChartOutput> GetOrderAmountAnalysisOutputAsync(OrderChartInput input)
        {
            var userId = await GetUserIdByEmailAsync(input.Email);
            //初始化订单分析结果数据
            var output = InitOrderAnalysisOutput(input);
            //初始化时间范围
            var (startTime, endTime) = ChartDateTimeHelper.InitTime(input.ChartDateType);
            //获取源数据
            string fieldOrDefaultSumValue = string.Empty;
            //按照币种统计，此时汇率输入框无论是否有值都对汇总金额没有影响。
            if (input.OrderAnalysisType == OrderAnalysisType.CurrencyType)
            {
                fieldOrDefaultSumValue = "a.FPaymentAmount";
            }
            else
            {
                fieldOrDefaultSumValue = string.IsNullOrWhiteSpace(input.ExchangeRate) ? "a.FPaymentAmount" : $"(CASE a.FCurrencyType when {(int)CurrencyType.USD} then a.FPaymentAmount*{input.ExchangeRate} else a.FPaymentAmount end)";
            }
            var orders = await GetOrderDataAsync(input, startTime, endTime, userId, fieldOrDefaultSumValue);
            //处理加工数据
            ProcessOrderAnalysisOutput(input, startTime, endTime, output, orders);
            return output;
        }

        /// <summary>
        /// 交易笔数分析(维度：支付渠道/业务类型/货币类型/平台类型/支付状态)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ChartOutput> GetPaymentCountAnalysisOutputAsync(PaymentChartInput input)
        {
            var userId = await GetUserIdByEmailAsync(input.Email);
            var output = InitPaymentAnalysisOutput(input);
            var (startTime, endTime) = ChartDateTimeHelper.InitTime(input.ChartDateType);
            var payments = await GetPaymentDataAsync(input, startTime, endTime, userId);
            ProcessPaymentAnalysisOutPut(input, startTime, endTime, output, payments);
            return output;
        }

        /// <summary>
        /// 交易金额分析(维度：支付渠道/业务类型/货币类型/平台类型/支付状态)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ChartOutput> GetPaymentAmountAnalysisOutputAsync(PaymentChartInput input)
        {
            var userId = await GetUserIdByEmailAsync(input.Email);
            var output = InitPaymentAnalysisOutput(input);
            var (startTime, endTime) = ChartDateTimeHelper.InitTime(input.ChartDateType);

            string fieldOrDefaultSumValue = string.Empty;
            //按照币种统计，此时汇率输入框无论是否有值都对汇总金额没有影响。
            if (input.PaymentAnalysisType == PaymentAnalysisType.CurrencyType)
            {
                fieldOrDefaultSumValue = "a.FPaymentAmount";
            }
            else
            {
                fieldOrDefaultSumValue = string.IsNullOrWhiteSpace(input.ExchangeRate) ? "a.FPaymentAmount" : $"(CASE a.FCurrencyType when {(int)CurrencyType.USD} then a.FPaymentAmount*{input.ExchangeRate} else a.FPaymentAmount end)";
            }
            var payments = await GetPaymentDataAsync(input, startTime, endTime, userId, fieldOrDefaultSumValue);
            ProcessPaymentAnalysisOutPut(input, startTime, endTime, output, payments);
            return output;
        }
        #endregion

    }
}