using System;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Sharding;

namespace YQTrack.Core.Backend.Admin.Seller.Service
{
    /// <summary>
    /// 分表映射定义
    /// </summary>
    public class SellerShardingMapper : ITableMappable, ITransientService
    {
        public string GetMappingTableName(Type modelType, object condition)
        {
            string ret = modelType.Name;
            if (condition is byte num)
            {
                if (num != 0)
                {
                    ret = $"{modelType.Name}{num.ToString().PadLeft(2, '0')}";
                }
            }
            else if (condition is DateTime date)
            {

                ret = $"{modelType.Name}{date.ToString("yyyyMM")}";
            }

            return ret;
        }
    }
}
