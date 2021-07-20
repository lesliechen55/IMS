using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Backend.Models;
using YQTrack.Backend.Models.Enums;
using YQTrack.Backend.Sharding;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Sharding;
using YQTrack.Core.Backend.Admin.Seller.Data;
using YQTrack.Core.Backend.Admin.Seller.Data.Models;
using YQTrack.Core.Backend.Admin.Seller.DTO.Input;
using YQTrack.Core.Backend.Admin.Seller.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Seller.Service.Imp
{
    public class UserShopService : IUserShopService
    {
        private readonly IDataAccessor<SellerOrderDBContext> _sellerOrderDataAccessor;
        private readonly ITableMappable _tableMapper;

        public UserShopService(IDataAccessor<SellerOrderDBContext> sellerOrderDataAccessor, SellerShardingMapper tableMapper)
        {
            _sellerOrderDataAccessor = sellerOrderDataAccessor;
            _tableMapper = tableMapper;
        }

        /// <summary>
        /// 更换分库/分表
        /// </summary>
        /// <returns></returns>
        private void SetDBRoute(DataRouteModel dataRouteModel, params Type[] mappingTypes)
        {
            var connectionString = DBShardingRouteFactory.GetDBConnStr(YQDbType.Seller.ToString(), dataRouteModel);
            SellerOrderDBContext.ConnectString = connectionString;
            List<TableMappingRule> rules = new List<TableMappingRule>();
            foreach (var mappingType in mappingTypes)
            {
                rules.Add(new TableMappingRule
                {
                    MappingType = mappingType,
                    Mapper = _tableMapper,
                    Condition = dataRouteModel.TableNo
                });
            }
            _sellerOrderDataAccessor.ChangeDataBase(connectionString);
        }

        /// <summary>
        /// 获取用户店铺分页列表
        /// </summary>
        /// <param name="input">用户店铺列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<UserShopPageDataOutput> outputs, int total)> GetPageDataAsync(UserShopPageDataInput input)
        {
            SetDBRoute(input.UserRoute.DataRouteModel, typeof(TUserShop));
            var queryable = _sellerOrderDataAccessor.GetQueryable<TUserShop>()
                .Where(w => w.FUserId == input.UserRoute.UserId)
                .WhereIf(() => input.FPlatformType.HasValue, x => x.FPlatformType == input.FPlatformType.Value)
                .WhereIf(() => input.FShopName.IsNotNullOrWhiteSpace(), x => x.FShopName.Contains(input.FShopName))
                .WhereIf(() => input.FState.HasValue, x => x.FState == input.FState);

            var count = await queryable.CountAsync();

            var outputs = await queryable
                .OrderBy(x => x.FState)
                .ThenByDescending(x => x.FCreateTime)
                .ProjectTo<UserShopPageDataOutput>()
                .ToPage(input.Page, input.Limit)
                .ToListAsync();

            return (outputs, count);
        }

        /// <summary>
        /// 获取店铺导入记录
        /// </summary>
        /// <param name="input">店铺导入记录搜索条件</param>
        /// <returns></returns>
        public async Task<IEnumerable<TrackUploadRecordOutput>> GetTrackUploadRecordAsync(TrackUploadRecordInput input)
        {
            SetDBRoute(input.UserRoute.DataRouteModel, typeof(TTrackUploadRecord));

            var queryable = _sellerOrderDataAccessor.GetQueryable<TTrackUploadRecord>()
                .Where(w => w.FShopId == input.FShopId);

            var outputs = await queryable
                .OrderByDescending(x => x.FCreateTime)
                .ProjectTo<TrackUploadRecordOutput>()
                .Take(15)
                .ToListAsync();

            return outputs;
        }
    }
}
