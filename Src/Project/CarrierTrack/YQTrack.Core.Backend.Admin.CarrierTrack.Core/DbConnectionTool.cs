using System;
using System.Collections.Generic;
using System.Linq;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Models;
using YQTrack.Backend.Models.Enums;
using YQTrack.Backend.Sharding;
using YQTrack.Core.Backend.Admin.Core;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.Core
{
    public static class DbConnectionTool
    {
        public static IEnumerable<string> GetAllDbConnections()
        {
            var dbTypeRoute = DBShardingRouteFactory.GetDBTypeRule(YQDbType.CarrierTrack.ToString());
            if (dbTypeRoute == null || !dbTypeRoute.NodeRoutes.Any())
            {
                throw new BusinessException($"{YQDbType.CarrierTrack.ToString()}当前数据库类型找不到相关分库配置");
            }
            var routeList = new List<DataRouteModel>();
            foreach (var (key, value) in dbTypeRoute.NodeRoutes)
            {
                routeList.AddRange(value.DBRules.Select(dbInfo => new DataRouteModel
                {
                    NodeId = (byte)key,
                    DbNo = (byte)dbInfo.Key,
                    TableNo = 0,
                    UserRole = (byte)UserRoleType.Carrier,
                    IsArchived = false,
                    IsWrite = true
                }));
            }
            var connections = routeList.Select(s => DBShardingRouteFactory.GetDBConnStr(YQDbType.CarrierTrack.ToString(), s)).ToList();
            if (!connections.Any())
            {
                throw new ArgumentNullException(nameof(connections));
            }
            return connections;
        }
    }
}
