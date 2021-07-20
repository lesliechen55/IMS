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
    public class TrackBatchTaskService : ITrackBatchTaskService
    {
        private readonly IDataAccessor<SellerOrderDBContext> _sellerOrderDataAccessor;
        private readonly ITableMappable _tableMapper;

        public TrackBatchTaskService(IDataAccessor<SellerOrderDBContext> sellerOrderDataAccessor, SellerShardingMapper tableMapper)
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
        /// 获取大批量任务分页列表
        /// </summary>
        /// <param name="input">大批量任务列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<BatchTaskPageDataOutput> outputs, int total)> GetPageDataAsync(BatchTaskPageDataInput input)
        {
            SetDBRoute(input.UserRoute.DataRouteModel, typeof(TTrackBatchTaskControl));
            var queryable = _sellerOrderDataAccessor.GetQueryable<TTrackBatchTaskControl>()
                .Where(w => w.FUserId == input.UserRoute.UserId)
                .WhereIf(() => input.FTaskType.HasValue, x => x.FTaskType == input.FTaskType.Value)
                .WhereIf(() => input.FTaskStatus.HasValue, x => x.FTaskStatus == input.FTaskStatus.Value);

            var count = await queryable.CountAsync();

            var outputs = await queryable
                .OrderByDescending(x => x.FCreateTime)
                .ProjectTo<BatchTaskPageDataOutput>()
                .ToPage(input.Page, input.Limit)
                .ToListAsync();

            return (outputs, count);
        }
    }
}
