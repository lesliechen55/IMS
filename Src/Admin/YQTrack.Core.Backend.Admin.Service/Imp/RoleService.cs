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

namespace YQTrack.Core.Backend.Admin.Service.Imp
{
    public class RoleService : IRoleService
    {
        private readonly AdminDbContext _dbContext;
        private readonly IMapper _mapper;

        public RoleService(AdminDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<RolePageDataOutput> Outputs, int Total)> GetPageDataAsync(string name, int page, int size)
        {
            var queryable = _dbContext.Role.WhereIf(() => !name.IsNullOrWhiteSpace(), x => x.FName.Contains(name.Trim()));
            var count = await queryable.CountAsync();
            var outputs = await queryable.OrderByDescending(x => x.FUpdatedTime).ThenBy(x => x.FName).ThenByDescending(x => x.FCreatedTime).ToPage(page, size).ProjectTo<RolePageDataOutput>().ToListAsync();
            return (outputs, count);
        }

        public async Task AddAsync(RoleAddInput input)
        {
            if (await _dbContext.Role.AnyAsync(x => x.FName == input.FName))
            {
                throw new BusinessException($"角色名字:{input.FName}已经存在");
            }
            var role = _mapper.Map<Role>(input);
            await _dbContext.Role.AddAsync(role);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(int id, string name, bool isActive, string remark, int updateBy)
        {
            var role = await _dbContext.Role.SingleOrDefaultAsync(x => x.FId == id);
            if (null == role)
            {
                throw new BusinessException($"参数{nameof(id)}错误:{id},数据库不存在");
            }
            role.FName = name;
            role.FIsActive = isActive;
            role.FRemark = remark;
            role.FUpdatedBy = updateBy;
            role.FUpdatedTime = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

        }

        public async Task<IEnumerable<RolePermissionOutput>> GetRolePermissionListAsync(int id)
        {
            var role = await _dbContext.Role.SingleOrDefaultAsync(x => x.FId == id);
            if (null == role)
            {
                throw new BusinessException($"参数{nameof(id)}错误:{id},数据库不存在");
            }
            var allPermission = await _dbContext.Permission.Where(x => x.FIsDeleted == false).ProjectTo<RolePermissionOutput>().ToListAsync();


            var rolePermissionIds = await (from rolePermission in _dbContext.RolePermission.Where(x => x.FRoleId == role.FId)
                                           join permission in _dbContext.Permission on rolePermission.FPermissionId equals permission.FId
                                           select permission.FId).ToListAsync();
            allPermission.ForEach(x => { x.IsSelect = rolePermissionIds.Contains(x.FId); });
            return allPermission;
        }

        public async Task SetPermissionListAsync(int roleId, int[] permissionIdList)
        {
            var role = await _dbContext.Role.SingleOrDefaultAsync(x => x.FId == roleId);
            if (null == role)
            {
                throw new BusinessException($"参数{nameof(roleId)}错误:{roleId},数据库不存在");
            }
            var permissionIds = await _dbContext.Permission.Where(x => x.FIsDeleted == false).Where(x => permissionIdList.Contains(x.FId)).Select(x => x.FId).ToListAsync();

            var existPermissionIds = await _dbContext.RolePermission.Where(x => x.FRoleId == role.FId).Select(x => x.FPermissionId).ToListAsync();

            var addPermissionIds = permissionIds.Except(existPermissionIds);

            var rolePermissions = addPermissionIds.Select(x => new RolePermission()
            {
                FRoleId = role.FId,
                FPermissionId = x
            }).ToArray();
            await _dbContext.RolePermission.AddRangeAsync(rolePermissions);

            var deletePermissionIds = existPermissionIds.Except(permissionIds);
            var deletePermissions = await _dbContext.RolePermission.Where(x => x.FRoleId == role.FId).Where(x => deletePermissionIds.Contains(x.FPermissionId)).ToArrayAsync();
            _dbContext.RolePermission.RemoveRange(deletePermissions);

            await _dbContext.SaveChangesAsync();

        }

        public async Task<(RolePageDataOutput output, string[] roleUserNameList)> GetByIdAsync(int id)
        {
            var role = await _dbContext.Role.SingleOrDefaultAsync(x => x.FId == id);
            if (null == role)
            {
                throw new BusinessException($"参数{nameof(id)}错误:{id},数据库不存在");
            }
            var output = _mapper.Map<RolePageDataOutput>(role);
            var accounts = await (from managerRole in _dbContext.ManagerRole
                                  join manager in _dbContext.Manager on managerRole.FManagerId equals manager.FId
                                  where managerRole.FRoleId == id
                                  orderby manager.FAccount
                                  select manager.FAccount).ToListAsync();
            return (output, accounts.ToArray());
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _dbContext.Role.SingleOrDefaultAsync(x => x.FId == id);
            if (null == role)
            {
                throw new BusinessException($"参数{nameof(id)}错误:{id},数据库不存在");
            }
            _dbContext.Role.Remove(role);
            var managerRoles = await _dbContext.ManagerRole.Where(x => x.FRoleId == id).ToListAsync();
            if (managerRoles.Any()) _dbContext.ManagerRole.RemoveRange(managerRoles);
            var rolePermissions = await _dbContext.RolePermission.Where(x => x.FRoleId == id).ToListAsync();
            if (rolePermissions.Any()) _dbContext.RolePermission.RemoveRange(rolePermissions);
            await _dbContext.SaveChangesAsync();
        }
    }
}