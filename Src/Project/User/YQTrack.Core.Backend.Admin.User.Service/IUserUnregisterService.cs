using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.DTO.Input;
using YQTrack.Core.Backend.Admin.User.DTO.Output;

namespace YQTrack.Core.Backend.Admin.User.Service
{
    public interface IUserUnregisterService : IScopeService
    {
        Task<(IEnumerable<UserUnregisterPageDataOutput> outputs, int total)> GetPageDataAsync(UserPageDataInput input);
    }
}