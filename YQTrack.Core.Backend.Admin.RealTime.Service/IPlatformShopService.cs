using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.RealTime.DTO.Input;
using YQTrack.Core.Backend.Admin.RealTime.DTO.Output;

namespace YQTrack.Core.Backend.Admin.RealTime.Service
{
    public interface IPlatformShopService : IScopeService
    {
        /// <summary>
        /// 获取平台店铺统计列表
        /// </summary>
        /// <param name="input">平台类型</param>
        /// <returns></returns>
        Task<(IEnumerable<PlatformShopDataOutput> outputs, int total)> GetDataAsync(PlatformShopDataInput input);

        /// <summary>
        /// 获取用户平台店铺统计列表
        /// </summary>
        /// <param name="input">平台类型</param>
        /// <returns></returns>
        Task<(IEnumerable<PlatformShopDataOutput> outputs, int total)> GetUserShopDataAsync(PlatformShopDataInput input);


        /// <summary>
        /// 获取付费用户平台店铺统计列表
        /// </summary>
        /// <param name="input">平台类型</param>
        /// <returns></returns>
        Task<(IEnumerable<PlatformShopDataOutput> outputs, int total)> GetPayingUsersDataAsync(PlatformShopDataInput input);


        /// <summary>
        /// 平台跟踪数量汇总(正常查询的查询数统计)
        /// </summary>
        /// <param name="input">平台类型</param>
        /// <returns></returns>
        Task<(IEnumerable<PlatformShopDataOutput> outputs, int total)> GetSearchNumDataAsync(PlatformShopDataInput input);
    }
}
