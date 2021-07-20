using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.Data;
using YQTrack.Core.Backend.Admin.User.DTO.Input;
using YQTrack.Core.Backend.Admin.User.DTO.Output;

namespace YQTrack.Core.Backend.Admin.User.Service.Imp
{
    public class UserUnregisterService : IUserUnregisterService
    {
        private readonly UserDbContext _userDbContext;

        public UserUnregisterService(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<(IEnumerable<UserUnregisterPageDataOutput> outputs, int total)> GetPageDataAsync(UserPageDataInput input)
        {
            var queryable = _userDbContext.TUserUnRegisterInfo
                .WhereIf(() => input.UserId.HasValue && input.UserId.Value > 0, x => x.FUserId == input.UserId.Value)
                .WhereIf(() => input.Email.IsNotNullOrWhiteSpace(), x => x.FEmail == input.Email);

            var count = await queryable.CountAsync();

            var outputs = await queryable
                .OrderByDescending(x => x.FUnRegisterTime)
                .ToPage(input.Page, input.Limit)
                .ProjectTo<UserUnregisterPageDataOutput>().ToListAsync();

            return (outputs, count);
        }
    }
}