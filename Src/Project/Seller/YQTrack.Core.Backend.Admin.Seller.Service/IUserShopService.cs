using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Seller.DTO.Input;
using YQTrack.Core.Backend.Admin.Seller.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Seller.Service
{
    public interface IUserShopService : IScopeService
    {
        //// <summary>
        /// 获取用户店铺分页列表
        /// <param name="input">用户店铺列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<UserShopPageDataOutput> outputs, int total)> GetPageDataAsync(UserShopPageDataInput input);

        /// <summary>
        /// 获取店铺导入记录
        /// </summary>
        /// <param name="input">店铺导入记录搜索条件</param>
        /// <returns></returns>
        Task<IEnumerable<TrackUploadRecordOutput>> GetTrackUploadRecordAsync(TrackUploadRecordInput input);
    }
}
