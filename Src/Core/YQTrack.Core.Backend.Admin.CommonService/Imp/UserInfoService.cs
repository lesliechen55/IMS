using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.Data;
using YQTrack.Core.Backend.Admin.User.Data.Models;

namespace YQTrack.Core.Backend.Admin.CommonService.Imp
{
    public class UserInfoService : IUserInfoService
    {
        private readonly UserDbContext _userDbContext;

        public UserInfoService(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        public async Task<long?> GetUserIdByEmailAsync(string email)
        {
            long? userId = null;
            if (email.IsNotNullOrWhiteSpace())
            {
                var tuserInfo = await _userDbContext.TuserInfo.SingleOrDefaultAsync(x => x.Femail == email.Trim());
                if (tuserInfo != null)
                {
                    userId = tuserInfo.FuserId;
                }
                else
                {
                    throw new BusinessException($"参数{nameof(email)}值错误,找不到对应用户");
                }
            }
            return userId;
        }

        public async Task<Dictionary<long, string>> GetEmailListByUserIdListAsync(long[] userIdList)
        {
            if (userIdList == null || !userIdList.Any()) return new Dictionary<long, string>();
            var list = await _userDbContext.TuserInfo.Where(x => userIdList.Contains(x.FuserId)).Select(x => new
            {
                x.FuserId,
                x.Femail
            }).ToListAsync();
            return list.ToDictionary(x => x.FuserId, x => x.Femail);
        }

        public async Task<TuserInfo> GetRequiredByIdAsync(long userId)
        {
            var tuserInfo = await _userDbContext.TuserInfo.SingleOrDefaultAsync(x => x.FuserId == userId);
            if (tuserInfo == null) throw new BusinessException($"{nameof(userId)}找不到对应注册用户数据");
            return tuserInfo;
        }


        public async Task<long?> GetUserInfo(string email)
        {
            long? userId = null;
            if (email.IsNotNullOrWhiteSpace())
            {
                var tuserInfo = await _userDbContext.TuserInfo.SingleOrDefaultAsync(x => x.Femail == email.Trim());
                if (tuserInfo != null)
                {
                    userId = tuserInfo.FuserId;
                }
                else
                {
                    throw new BusinessException($"参数{nameof(email)}值错误,找不到对应用户");
                }
            }
            return userId;
        }

        //获取用户名称
        public async Task<Dictionary<long, string>> GetUserNickName(long[] userIdList)
        {
            try
            {
                if (userIdList == null || !userIdList.Any()) return new Dictionary<long, string>();
                var list = await _userDbContext.TuserInfo.Where(x => userIdList.Contains(x.FuserId)).Select(x => new
                {
                    x.FuserId,
                    x.FnickName
                }).ToListAsync();
                return list.ToDictionary(x => x.FuserId, x => x.FnickName);
            }
            catch (Exception ex)
            {
                //throw new BusinessException($"GetUserNickName:" +ex.Message);
                return new Dictionary<long, string>();
            }
        }

    }
}