using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data;
using YQTrack.Core.Backend.Admin.Data.Entity;
using YQTrack.Core.Backend.Admin.DTO.Input;
using YQTrack.Core.Backend.Admin.DTO.Output;
using YQTrack.Utility;

namespace YQTrack.Core.Backend.Admin.Service.Imp
{
    public class HomeService : IHomeService
    {
        private readonly AdminDbContext _dbContext;
        private readonly IMapper _mapper;

        public HomeService(AdminDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<(int UserId, string Account, string NickName)> LoginAsync(string account, string pwd, string ip, string userAgent, string platForm, bool isLog)
        {
            var manager = await _dbContext.Manager.SingleOrDefaultAsync(x => x.FAccount == account);
            if (manager == null || manager.FPassword != SecurityExtend.MD5Encrypt(pwd))
            {
                throw new BusinessException("帐号或者密码错误");
            }
            if (manager.FIsLock)
            {
                throw new BusinessException("帐号被禁用,请找其他管理员帮你启用");
            }
            if (manager.FIsDeleted)
            {
                throw new BusinessException("帐号被删除,无法使用");
            }
            manager.FLastLoginTime = DateTime.UtcNow;
            if (isLog)
            {
                userAgent = userAgent.Length > 128 ? userAgent.Substring(0, 128) : userAgent;//处理因为字符串过长报错

                await _dbContext.LoginLog.AddAsync(new LoginLog()
                {
                    FAccount = manager.FAccount,
                    FCreatedBy = manager.FId,
                    FIp = ip,
                    FManagerId = manager.FId,
                    FNickName = manager.FNickName,
                    FPlatform = platForm ?? "web",
                    FUserAgent = userAgent
                });
            }
            await _dbContext.SaveChangesAsync();
            return (manager.FId, manager.FAccount, manager.FNickName);
        }

        public IEnumerable<string> GetExistPermissionList(int managerId)
        {
            var manager = _dbContext.Manager.SingleOrDefault(x => x.FId == managerId && x.FIsDeleted == false && x.FIsLock == false);
            if (null == manager || manager.FIsDeleted || manager.FIsLock) return new List<string>();

            var existPermissions = (from managerRole in _dbContext.ManagerRole
                                    join role in _dbContext.Role on managerRole.FRoleId equals role.FId
                                    join rolePermission in _dbContext.RolePermission on role.FId equals rolePermission.FRoleId
                                    join permission in _dbContext.Permission on rolePermission.FPermissionId equals permission.FId
                                    where managerRole.FManagerId == manager.FId &&
                                          role.FIsActive && !role.FIsDeleted &&
                                          !permission.FIsDeleted && permission.FFullName != null
                                    select permission.FFullName).Distinct().ToList();
            return existPermissions;
        }

        public async Task<(int totalUser, int totalRole, int totalPermission, DateTime lastLoginTime)> GetMainDataAsync(int loginId)
        {
            var manager = await _dbContext.Manager.SingleOrDefaultAsync(x => x.FId == loginId);
            if (manager == null)
            {
                throw new BusinessException($"参数错误:{nameof(loginId)}={loginId} 管理员不存在");
            }
            if (manager.FIsLock)
            {
                throw new BusinessException("帐号被禁用,请找其他管理员帮你禁用");
            }
            if (manager.FIsDeleted)
            {
                throw new BusinessException("帐号被删除,无法使用");
            }
            var totalUser = await _dbContext.Manager.Where(x => !x.FIsDeleted).CountAsync();
            var totalRole = await _dbContext.Role.Where(x => !x.FIsDeleted).CountAsync();
            var totalPermission = await _dbContext.Permission.Where(x => !x.FIsDeleted).CountAsync();
            return (totalUser, totalRole, totalPermission, manager.FLastLoginTime);
        }

        /// <summary>
        /// 根据当前登陆ID动态获取菜单(找出属于当前登录者的顶级菜单以及二三级菜单渲染即可)
        /// </summary>
        /// <param name="managerId"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, MenuOutput[]>> GetMenuAsync(int managerId)
        {
            var outputs = await (from manager in _dbContext.Manager
                                 join managerRole in _dbContext.ManagerRole on manager.FId equals managerRole.FManagerId
                                 join role in _dbContext.Role on managerRole.FRoleId equals role.FId
                                 join rolePermission in _dbContext.RolePermission on role.FId equals rolePermission.FRoleId
                                 join permission in _dbContext.Permission on rolePermission.FPermissionId equals permission.FId
                                 where manager.FId == managerId && !manager.FIsLock && !manager.FIsDeleted &&
                                       role.FIsActive && !role.FIsDeleted &&
                                       !permission.FIsDeleted
                                 orderby permission.FSort, permission.FUpdatedTime descending, permission.FCreatedTime descending
                                 select permission).Distinct().ProjectTo<PermissionOutput>().ToListAsync();

            var dictionary = new Dictionary<string, MenuOutput[]>();

            // 顶级菜单(只使用key)
            var tops = outputs.Where(x => x.FMenuType == MenuType.TopMenu && !x.FTopMenuKey.IsNullOrWhiteSpace()).OrderBy(x => x.FSort).ToList();

            foreach (var top in tops)
            {
                // 菜单组(只使用title+icon)
                var menuGroups = new List<MenuOutput>();

                var seconds = outputs.Where(x => x.FParentId.HasValue && x.FParentId.Value == top.FId && x.FMenuType == MenuType.MenuGroup).OrderBy(x => x.FSort).ToList();

                foreach (var second in seconds)
                {
                    var menuOutput = new MenuOutput
                    {
                        Title = second.FName,
                        Icon = second.FIcon ?? "&#xe630;",
                        Href = string.Empty,
                        Spread = false
                    };

                    // 寻找三级菜单挂载到二级菜单
                    var childArray = outputs.Where(x => x.FParentId.HasValue && x.FParentId.Value == second.FId && x.FMenuType == MenuType.Function).OrderBy(x => x.FSort).Select(x => new MenuOutput
                    {
                        Title = x.FName,
                        Icon = x.FIcon ?? "&#xe630;",
                        Href = x.FUrl,
                        Spread = false
                    }).ToArray();

                    menuOutput.Children = childArray;

                    menuGroups.Add(menuOutput);
                }

                dictionary.Add(top.FTopMenuKey.ToLower(), menuGroups.ToArray());
            }

            return dictionary;
        }

        public async Task<(Dictionary<string, string> TopKeyAndNameDic, string defaultSelectedTopKey, string Avatar)> GetTopKeyAndNameDicAsync(int managerId)
        {
            var managerAdmin = await _dbContext.Manager.SingleOrDefaultAsync(x => x.FId == managerId);
            if (managerAdmin == null)
            {
                throw new BusinessException($"{nameof(managerId)}参数错误,{nameof(managerAdmin)}数据库不存在");
            }

            var outputs = await (from manager in _dbContext.Manager
                                 join managerRole in _dbContext.ManagerRole on manager.FId equals managerRole.FManagerId
                                 join role in _dbContext.Role on managerRole.FRoleId equals role.FId
                                 join rolePermission in _dbContext.RolePermission on role.FId equals rolePermission.FRoleId
                                 join permission in _dbContext.Permission on rolePermission.FPermissionId equals permission.FId
                                 where manager.FId == managerId && !manager.FIsLock && !manager.FIsDeleted &&
                                       role.FIsActive && !role.FIsDeleted &&
                                       !permission.FIsDeleted && permission.FMenuType == MenuType.TopMenu && permission.FTopMenuKey != null
                                 orderby permission.FSort, permission.FUpdatedTime descending, permission.FCreatedTime descending
                                 select new
                                 {
                                     permission.FTopMenuKey,
                                     permission.FName,
                                     permission.FSort
                                 }).Distinct().ToListAsync();

            var topKeyAndNameDic = outputs.OrderBy(x => x.FSort).ToDictionary(x => x.FTopMenuKey.ToLower(), x => x.FName);
            var defaultSelectedTopKey = topKeyAndNameDic.Any() ? topKeyAndNameDic.First().Key : string.Empty;
            return (topKeyAndNameDic, defaultSelectedTopKey, managerAdmin.FAvatar);
        }

        public async Task<(IEnumerable<LoginLogPageDataOutput> outputs, int total)> GetLoginLogPageDataAsync(LoginLogPageDataInput input)
        {
            var query = _dbContext.LoginLog
                .WhereIf(() => !input.Account.IsNullOrWhiteSpace(), x => x.FAccount.Contains(input.Account.Trim()))
                .WhereIf(() => !input.NickName.IsNullOrWhiteSpace(), x => x.FNickName.Contains(input.NickName.Trim()))
                .WhereIf(() => !input.Platform.IsNullOrWhiteSpace(), x => x.FPlatform == input.Platform.Trim());
            var count = await query.CountAsync();
            var outputs = await query
                .OrderByDescending(x => x.FCreatedTime).ToPage(input.Page, input.Limit).ProjectTo<LoginLogPageDataOutput>().ToListAsync();
            return (outputs, count);
        }

        public async Task<(IEnumerable<OperationLogPageDataOutput> outputs, int total)> GetOperationLogPageDataAsync(OperationLogPageDataInput input)
        {
            var query = _dbContext.OperationLog
                .WhereIf(() => !input.Account.IsNullOrWhiteSpace(), x => x.FAccount.Contains(input.Account.Trim()))
                .WhereIf(() => !input.Method.IsNullOrWhiteSpace(), x => x.FMethod.Contains(input.Method.Trim()))
                .WhereIf(() => !input.Desc.IsNullOrWhiteSpace(), x => x.FDesc.Contains(input.Desc.Trim()))
                .WhereIf(() => input.OperationType.HasValue, x => x.FOperationType == input.OperationType.Value)
                .WhereIf(() => input.StartDate.HasValue, x => x.FCreatedTime >= input.StartDate.Value.Date)
                .WhereIf(() => input.EndDate.HasValue, x => x.FCreatedTime < input.EndDate.Value.AddDays(1).Date);
            var count = await query.CountAsync();
            var outputs = await query.OrderByDescending(x => x.FCreatedTime).ToPage(input.Page, input.Limit).ProjectTo<OperationLogPageDataOutput>().ToListAsync();
            return (outputs, count);
        }

        public async Task<bool> HasSuperRoleAsync(int managerId)
        {
            var roleIds = await (from manager in _dbContext.Manager
                                 join managerRole in _dbContext.ManagerRole on manager.FId equals managerRole.FManagerId
                                 join role in _dbContext.Role on managerRole.FRoleId equals role.FId
                                 where manager.FId == managerId && manager.FIsLock == false && manager.FIsDeleted == false
                                       && role.FIsActive && role.FIsDeleted == false
                                 select role.FId).Distinct().ToListAsync();
            // 注意:约定RoleId=1表示系统超级角色
            return roleIds.Any(x => x == 1);
        }
    }
}