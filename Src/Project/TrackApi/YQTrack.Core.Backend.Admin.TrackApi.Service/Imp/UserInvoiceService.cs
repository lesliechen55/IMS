using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.TrackApi.Data;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Input;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;
using YQTrack.Core.Backend.Admin.Core;
using AutoMapper.QueryableExtensions;

namespace YQTrack.Core.Backend.Admin.TrackApi.Service.Imp
{
    public class UserInvoiceService : IUserInvoiceService
    {
        private readonly ApiUserDbContext _dbApiUserContext;

        public UserInvoiceService(ApiUserDbContext dbApiUserContext)
        {
            _dbApiUserContext = dbApiUserContext;
        }

        /// <summary>
        /// 获取账单列表
        /// </summary>
        /// <param name="input">账单列表搜索条件</param>
        /// <returns></returns>
        public async Task<IEnumerable<UserInvoiceOutput>> GetListDataAsync(UserInvoiceInput input)
        {
            var output = await _dbApiUserContext.TApiUserInvoice
                .WhereIf(() => input.UserId.HasValue, x => x.FUserId == input.UserId.Value)
                .WhereIf(() => input.StartTime.HasValue, x => x.FStartDate >= input.StartTime.Value)
                .WhereIf(() => input.EndTime.HasValue, x => x.FStartDate < input.EndTime.Value.AddMonths(1))
                .OrderByDescending(o => o.FInvoiceId)
                .ProjectTo<UserInvoiceOutput>()
                .ToListAsync();

            return output;
        }
    }
}
