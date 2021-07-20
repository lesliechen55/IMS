using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.RealTime.DTO.Input;
using YQTrack.Core.Backend.Admin.RealTime.DTO.Output;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.RealTime.Data.Models;
using YQTrack.Core.Backend.Admin.RealTime.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using YQTrack.Core.Backend.Admin.Core.Sharding;
using YQTrack.Backend.Sharding;
using YQTrack.Backend.Models.Enums;
using YQTrack.Backend.Models;
using AutoMapper.QueryableExtensions;
using YQTrack.Core.Backend.Enums.User;
using YQTrack.Backend.Sharding.Router;
using YQTrack.Core.Backend.Admin.Seller.Data;
using MongoDB.Bson.IO;

namespace YQTrack.Core.Backend.Admin.RealTime.Service.Imp
{
    public class PlatformShopService : IPlatformShopService
    {
        private readonly IDataAccessor<RealTimeDbContext> _sellerOrderDataAccessor;
        private readonly IMapper _mapper;

        public PlatformShopService(IDataAccessor<RealTimeDbContext> sellerOrderDataAccessor, IMapper mapper)
        {
            _sellerOrderDataAccessor = sellerOrderDataAccessor;
            _mapper = mapper;
        }

        //获取平台店铺统计列表
        public async Task<(IEnumerable<PlatformShopDataOutput> outputs, int total)> GetDataAsync(PlatformShopDataInput input)
        {
            List<PlatformShopDataOutput> listData = new List<PlatformShopDataOutput>();

            var strDbBuyer = Enum.GetName(typeof(YQDbType), YQDbType.Seller);
            var routeList = DBShardingRouteFactory.GetDataRouteModels(strDbBuyer);
            foreach (var item in routeList)
            {
                var dbNo = Convert.ToByte(item.DbNo);
                var dataRouteModel = new DataRouteModel
                {
                    UserRole = (byte)item.UserRole,
                    NodeId = Convert.ToByte(item.NodeId),
                    DbNo = dbNo,
                    TableNo = Convert.ToByte(item.TableNo),
                    IsWrite=true
                };

                #region 获取分表后的连接字符串 
                var connectionString = DBShardingRouteFactory.GetDBConnStr(YQDbType.Seller.ToString(), dataRouteModel);
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    var dBRules = DBShardingRouteFactory.DicDBTypeNodes["seller"].NodeRoutes.ToArray()[0].Value.DBRules.Values;
                    var connStr = dBRules.ToArray().Where(e => e.DBNo == dbNo).First().WriteConStr.ConnStr;
                    connectionString = connStr;

                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new BusinessException("数据库连接字符串为空:" + Newtonsoft.Json.JsonConvert.SerializeObject(dBRules));
                    }
                }
                RealTimeDbContext.ConnectString = connectionString;
                _sellerOrderDataAccessor.ChangeDataBase(connectionString);
                #endregion

                #region 查询
                var where = PredicateBuilder.True<TUserShop>().And(a => a.FState == 0);

                var queryable = _sellerOrderDataAccessor.GetQueryable<TUserShop>().Where(where);

                var outputs = (await queryable
                    .GroupBy(e => e.FPlatformType)
                   .ToListAsync())
                    .Select(
                            e =>
                        new PlatformShopDataOutput
                        {
                            PlatformType = e.Key,
                            SumCount = e.Count()
                        }
                    )
                    ;

                #endregion

                listData = listData.Union(outputs).ToList();
            }

            var result = listData
                 .WhereIf(() => input.PlatformType.HasValue && input.PlatformType != -1, x => x.PlatformType == input.PlatformType.Value)
                .OrderBy(z => z.PlatformType).GroupBy(z => z.PlatformType)
                    .Select(
                            e =>
                        new PlatformShopDataOutput
                        {
                            PlatformType = e.Key,
                            SumCount = e.Sum(ss => ss.SumCount)
                        }
                    );

            var results = _mapper.Map<IEnumerable<PlatformShopDataOutput>>(result);
            return (results, results.Count());
        }

        //获取用户平台店铺统计列表
        public async Task<(IEnumerable<PlatformShopDataOutput> outputs, int total)> GetUserShopDataAsync(PlatformShopDataInput input)
        {
            List<UserPlatformShopDataOutput> listData = new List<UserPlatformShopDataOutput>();

            var strDbBuyer = Enum.GetName(typeof(YQDbType), YQDbType.Seller);
            var routeList = DBShardingRouteFactory.GetDataRouteModels(strDbBuyer);

            foreach (var item in routeList)
            {
                var dbNo = Convert.ToByte(item.DbNo);
                var dataRouteModel = new DataRouteModel
                {
                    UserRole = (byte)item.UserRole,
                    NodeId = Convert.ToByte(item.NodeId),
                    DbNo = dbNo,
                    TableNo = Convert.ToByte(item.TableNo),
                    IsWrite = true
                };

                #region 获取分表后的连接字符串 
                var connectionString = DBShardingRouteFactory.GetDBConnStr(YQDbType.Seller.ToString(), dataRouteModel);
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    var dBRules = DBShardingRouteFactory.DicDBTypeNodes["seller"].NodeRoutes.ToArray()[0].Value.DBRules.Values;
                    var connStr = dBRules.ToArray().Where(e => e.DBNo == dbNo).First().WriteConStr.ConnStr;
                    connectionString = connStr;

                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new BusinessException("数据库连接字符串为空:" + Newtonsoft.Json.JsonConvert.SerializeObject(dBRules));
                    }
                }
                RealTimeDbContext.ConnectString = connectionString;
                _sellerOrderDataAccessor.ChangeDataBase(connectionString);
                #endregion

                #region 查询  
                var fState = Enums.Seller.ShopStateType.Delete;
                var where = PredicateBuilder.True<TUserShop>().And(a => a.FState != fState).And(a => a.FPlatformType >= 0);

                var queryable = _sellerOrderDataAccessor.GetQueryable<TUserShop>().Where(where);

                var outputs = (await queryable
                    .GroupBy(q => new { q.FPlatformType, q.FUserId })
                    .ToListAsync())
                    .Select(
                            e =>
                        new UserPlatformShopDataOutput
                        {
                            PlatformType = e.Key.FPlatformType,
                            FUserId = e.Key.FUserId
                        }
                    );

                listData = listData.Union(outputs).ToList();

                #endregion
            }

            #region 去重(Distinct()方法没有用,只能这样操作了)
            List<UserPlatformShopDataOutput> listDataNew = new List<UserPlatformShopDataOutput>();
            listDataNew = listData.GroupBy(q => new { q.PlatformType, q.FUserId }).Select(e => new UserPlatformShopDataOutput
            {
                PlatformType = e.Key.PlatformType,
                FUserId = e.Key.FUserId
            }).ToList();
            #endregion

            var result = listDataNew
                 .WhereIf(() => input.PlatformType.HasValue && input.PlatformType != -1, x => x.PlatformType == input.PlatformType.Value)
                .GroupBy(z => z.PlatformType)
                    .Select(
                            e =>
                        new PlatformShopDataOutput
                        {
                            PlatformType = e.Key,
                            SumCount = e.Count()
                        }
                    );

            var results = _mapper.Map<IEnumerable<PlatformShopDataOutput>>(result);
            return (results, results.Count());
        }

        //获取付费用户平台店铺统计列表
        public async Task<(IEnumerable<PlatformShopDataOutput> outputs, int total)> GetPayingUsersDataAsync(PlatformShopDataInput input)
        {
            List<PlatformShopDataOutput> listData = new List<PlatformShopDataOutput>();

            var strDbBuyer = Enum.GetName(typeof(YQDbType), YQDbType.Seller);
            var routeList = DBShardingRouteFactory.GetDataRouteModels(strDbBuyer);
            foreach (var item in routeList)
            {
                var dbNo = Convert.ToByte(item.DbNo);
                var dataRouteModel = new DataRouteModel
                {
                    UserRole = (byte)item.UserRole,
                    NodeId = Convert.ToByte(item.NodeId),
                    DbNo = dbNo,
                    TableNo = Convert.ToByte(item.TableNo),
                    IsWrite = true
                };

                #region 获取分表后的连接字符串
                var connectionString = DBShardingRouteFactory.GetDBConnStr(YQDbType.Seller.ToString(), dataRouteModel);
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    var dBRules = DBShardingRouteFactory.DicDBTypeNodes["seller"].NodeRoutes.ToArray()[0].Value.DBRules.Values;
                    var connStr = dBRules.ToArray().Where(e => e.DBNo == dbNo).First().WriteConStr.ConnStr;
                    connectionString = connStr;

                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new BusinessException("数据库连接字符串为空:" + Newtonsoft.Json.JsonConvert.SerializeObject(dBRules));
                    }
                }
                RealTimeDbContext.ConnectString = connectionString;
                _sellerOrderDataAccessor.ChangeDataBase(connectionString);
                #endregion

                #region 查询  

                var businessCtrlList = _sellerOrderDataAccessor.GetQueryable<TBusinessCtrl>().Where(e => e.FPurchaseOrderId > 0).Select(e => e.FUserId).ToList();

                var fState = Enums.Seller.ShopStateType.Delete;
                var where = PredicateBuilder.True<TUserShop>().And(a => a.FState != fState).And(a => a.FPlatformType >= 0)
                    .And(a => businessCtrlList.Contains(a.FUserId));

                var queryable = _sellerOrderDataAccessor.GetQueryable<TUserShop>().Where(where);

                var outputs = (await queryable
                    .GroupBy(q => new { q.FPlatformType, q.FUserId })
                    .ToListAsync())
                    .Select(
                            e =>
                        new UserPlatformShopDataOutput
                        {
                            PlatformType = e.Key.FPlatformType,
                            FUserId = e.Key.FUserId
                        }
                    );

                var platformUserList = outputs.GroupBy(e => e.PlatformType)
                    .OrderBy(e => e.Key).Select(e => new PlatformShopDataOutput
                    {
                        PlatformType = e.Key,
                        SumCount = e.Count()
                    });


                #endregion

                listData = listData.Union(platformUserList).ToList();
            }

            var result = listData
                 .WhereIf(() => input.PlatformType.HasValue && input.PlatformType != -1, x => x.PlatformType == input.PlatformType.Value)
                .OrderBy(z => z.PlatformType).GroupBy(z => z.PlatformType)
                    .Select(
                            e =>
                        new PlatformShopDataOutput
                        {
                            PlatformType = e.Key,
                            SumCount = e.Sum(ss => ss.SumCount)
                        }
                    );

            var results = _mapper.Map<IEnumerable<PlatformShopDataOutput>>(result);
            return (results, results.Count());
        }

        //平台跟踪数量汇总(正常查询的查询数统计)
        public async Task<(IEnumerable<PlatformShopDataOutput> outputs, int total)> GetSearchNumDataAsync(PlatformShopDataInput input)
        {
            await Task.Delay(1);
            List<PlatformShopDataOutput> listData = new List<PlatformShopDataOutput>();
            List<int> numList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };

            var strDbBuyer = Enum.GetName(typeof(YQDbType), YQDbType.Seller);
            var routeList = DBShardingRouteFactory.GetDataRouteModels(strDbBuyer);
            foreach (var item in routeList)
            {
                var dbNo = Convert.ToByte(item.DbNo);
                var dataRouteModel = new DataRouteModel
                {
                    UserRole = (byte)item.UserRole,
                    NodeId = Convert.ToByte(item.NodeId),
                    DbNo = dbNo,
                    TableNo = Convert.ToByte(item.TableNo),
                    IsWrite = true
                };
                #region 获取分表后的连接字符串
                var connectionString = DBShardingRouteFactory.GetDBConnStr(YQDbType.Seller.ToString(), dataRouteModel);
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    var dBRules = DBShardingRouteFactory.DicDBTypeNodes["seller"].NodeRoutes.ToArray()[0].Value.DBRules.Values;
                    var connStr = dBRules.ToArray().Where(e => e.DBNo == dbNo).First().WriteConStr.ConnStr;
                    connectionString = connStr;

                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new BusinessException("数据库连接字符串为空:" + Newtonsoft.Json.JsonConvert.SerializeObject(dBRules));
                    }
                }
                RealTimeDbContext.ConnectString = connectionString;
                _sellerOrderDataAccessor.ChangeDataBase(connectionString);
                #endregion

                #region 查询  

                var fState = Enums.Seller.ShopStateType.Delete;
                var contextUserShop = _sellerOrderDataAccessor.GetQueryable<TUserShop>();
                var contextTrackInfo = _sellerOrderDataAccessor.GetQueryable<TTrackInfo>();

                var searchNumList = from o in contextUserShop
                                    join d in contextTrackInfo
                                    on o.FShopId equals d.FShopId

                                    where d.FTrackStateType == 2 && d.FArchivedState == 0 && numList.Contains(o.FPlatformType)
                                    && o.FState != fState
                                    group o by o.FPlatformType into g
                                    select new PlatformShopDataOutput
                                    {
                                        PlatformType = g.Key,
                                        SumCount = g.Count()
                                    };

                #endregion

                listData = listData.Union(searchNumList).ToList();
            }

            var result = listData
                 .WhereIf(() => input.PlatformType.HasValue && input.PlatformType != -1, x => x.PlatformType == input.PlatformType.Value)
                .OrderBy(z => z.PlatformType).GroupBy(z => z.PlatformType)
                    .Select(
                            e =>
                        new PlatformShopDataOutput
                        {
                            PlatformType = e.Key,
                            SumCount = e.Sum(ss => ss.SumCount)
                        }
                    );

            var results = _mapper.Map<IEnumerable<PlatformShopDataOutput>>(result);
            return (results, results.Count());
        }
    }
}
