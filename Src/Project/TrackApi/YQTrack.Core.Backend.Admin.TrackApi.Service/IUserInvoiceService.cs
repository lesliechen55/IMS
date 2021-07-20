using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Input;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;

namespace YQTrack.Core.Backend.Admin.TrackApi.Service
{
    public interface IUserInvoiceService : IScopeService
    {
        /// <summary>
        /// 获取账单列表
        /// </summary>
        /// <param name="input">账单列表搜索条件</param>
        /// <returns></returns>
        Task<IEnumerable<UserInvoiceOutput>> GetListDataAsync(UserInvoiceInput input);
    }
}
