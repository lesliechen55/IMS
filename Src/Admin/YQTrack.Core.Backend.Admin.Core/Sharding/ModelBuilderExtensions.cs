﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Core.Sharding
{
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// 根据传入规则改变数据表的映射
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="mapping">映射规则</param>
        public static void ChangeTableMapping(this ModelBuilder builder, ICollection<TableMappingRule> mapping)
        {
            if (mapping != null)
            {
                string tableNm;
                foreach (var rule in mapping)
                {
                    tableNm = rule.Mapper.GetMappingTableName(rule.MappingType, rule.Condition);
                    builder.Entity(rule.MappingType).ToTable(tableNm);
                }
            }
        }
    }
}
