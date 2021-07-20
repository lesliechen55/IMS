using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Data;
using YQTrack.Core.Backend.Admin.Data.Entity;
using YQTrack.Core.Backend.Admin.DTO.Input;
using YQTrack.Core.Backend.Admin.DTO.Output;
using YQTrack.Utility;

namespace YQTrack.Core.Backend.Admin.Service.Imp
{
    public class ManagerService : IManagerService
    {
        private readonly AdminDbContext _dbContext;
        private readonly IMapper _mapper;

        public ManagerService(AdminDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<ManagerPageDataOutput> Outputs, int Total)> GetPageDataAsync(ManagerPageDataInput input)
        {
            var query = _dbContext.Manager
                .WhereIf(() => !input.Account.IsNullOrWhiteSpace(), x => x.FAccount.Contains(input.Account.Trim()))
                .WhereIf(() => !input.NickName.IsNullOrWhiteSpace(), x => x.FNickName.Contains(input.NickName.Trim()));
            var count = await query.CountAsync();
            var list = await query.OrderByDescending(x => x.FUpdatedTime).ThenByDescending(x => x.FCreatedTime).ToPage(input.Page, input.Limit).ProjectTo<ManagerPageDataOutput>().ToListAsync();
            return (list, count);
        }

        public async Task AddAsync(ManagerAddInput input, int createBy)
        {
            if (await _dbContext.Manager.AnyAsync(x => x.FAccount == input.FAccount))
            {
                throw new BusinessException($"{nameof(input.FAccount)}不允许重复");
            }
            var manager = _mapper.Map<Manager>(input);
            manager.FPassword = SecurityExtend.MD5Encrypt(input.FPassword.Trim());
            manager.FCreatedBy = createBy;
            await _dbContext.Manager.AddAsync(manager);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(int id, string nickName, string pwd, bool isLock, int updateBy, string remark)
        {
            var manager = await _dbContext.Manager.SingleOrDefaultAsync(x => x.FId == id);
            if (null == manager)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(manager)}数据库不存在");
            }
            manager.FNickName = nickName;
            if (!pwd.IsNullOrWhiteSpace())
            {
                manager.FPassword = SecurityExtend.MD5Encrypt(pwd.Trim());
            }
            manager.FIsLock = isLock;
            manager.FUpdatedBy = updateBy;
            manager.FUpdatedTime = DateTime.UtcNow;
            manager.FRemark = remark;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ManagerRoleOutput>> GetRoleListAsync(int id)
        {
            var manager = await _dbContext.Manager.SingleOrDefaultAsync(x => x.FId == id);
            if (null == manager)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(manager)}数据库不存在");
            }
            var roles = await _dbContext.Role.Where(x => x.FIsDeleted == false && x.FIsActive).ProjectTo<ManagerRoleOutput>().ToListAsync();
            var managerRoleIds = await (from managerRole in _dbContext.ManagerRole
                                        join role in _dbContext.Role on managerRole.FRoleId equals role.FId
                                        where managerRole.FManagerId == manager.FId
                                        select role.FId).ToListAsync();
            roles.ForEach(x => { x.IsSelect = managerRoleIds.Contains(x.FId); });
            return roles;
        }

        public async Task SetRoleListAsync(int userId, int[] roleIdList)
        {
            var manager = await _dbContext.Manager.SingleOrDefaultAsync(x => x.FId == userId);
            if (null == manager)
            {
                throw new BusinessException($"{nameof(userId)}参数错误,{nameof(manager)}数据库不存在");
            }
            var roleIds = await _dbContext.Role.Where(x => x.FIsActive && x.FIsDeleted == false).Where(x => roleIdList.Contains(x.FId)).Select(x => x.FId).ToListAsync();

            var existRoleIds = await _dbContext.ManagerRole.Where(x => x.FManagerId == manager.FId).Select(x => x.FRoleId).ToListAsync();

            var addRoleIds = roleIds.Except(existRoleIds);

            var addRoles = addRoleIds.Select(x => new ManagerRole { FManagerId = manager.FId, FRoleId = x }).ToArray();
            await _dbContext.ManagerRole.AddRangeAsync(addRoles);

            var deleteRoleIds = existRoleIds.Except(roleIds).ToArray();

            var deleteRoles = await _dbContext.ManagerRole.Where(x => x.FManagerId == manager.FId).Where(x => deleteRoleIds.Contains(x.FRoleId)).ToArrayAsync();

            _dbContext.ManagerRole.RemoveRange(deleteRoles);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ManagerPageDataOutput> GetByIdAsync(int id)
        {
            var manager = await _dbContext.Manager.SingleOrDefaultAsync(x => x.FId == id);
            if (null == manager)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(manager)}数据库不存在");
            }
            var output = _mapper.Map<ManagerPageDataOutput>(manager);
            return output;
        }

        public async Task ChangePwdAsync(int id, string newPwd, string oldPwd)
        {
            var manager = await _dbContext.Manager.SingleOrDefaultAsync(x => x.FId == id);
            if (null == manager)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(manager)}数据库不存在");
            }

            if (!manager.FPassword.Equals(SecurityExtend.MD5Encrypt(oldPwd), StringComparison.InvariantCultureIgnoreCase))
            {
                throw new BusinessException($"{nameof(oldPwd)}参数错误,原始密码错误");
            }

            manager.FPassword = SecurityExtend.MD5Encrypt(newPwd.Trim());
            manager.FUpdatedTime = DateTime.UtcNow;
            manager.FUpdatedBy = id;
            await _dbContext.SaveChangesAsync();
        }

        private async Task<Manager> CheckExistByIdAsync(int id)
        {
            var manager = await _dbContext.Manager.SingleOrDefaultAsync(x => x.FId == id);
            if (null == manager)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(manager)}数据库不存在");
            }
            return manager;
        }

        public async Task UpdateNickNameAsync(int id, string nickName)
        {
            var manager = await CheckExistByIdAsync(id);
            manager.FNickName = nickName;
            manager.FUpdatedBy = id;
            manager.FUpdatedTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAvatarAsync(int id, string path)
        {
            var manager = await CheckExistByIdAsync(id);
            manager.FAvatar = path;
            manager.FUpdatedBy = id;
            manager.FUpdatedTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var manager = await CheckExistByIdAsync(id);
            _dbContext.Manager.Remove(manager);
            await _dbContext.SaveChangesAsync();
        }
    }
}