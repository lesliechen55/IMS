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

namespace YQTrack.Core.Backend.Admin.Service.Imp
{
    public class PermissionService : IPermissionService
    {
        private readonly AdminDbContext _dbContext;
        private readonly IMapper _mapper;

        public PermissionService(AdminDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionOutput>> GetAllAsync()
        {
            var outputs = await _dbContext.Permission.Where(x => x.FIsDeleted == false).OrderBy(x => x.FSort).ThenByDescending(x => x.FUpdatedTime).ThenByDescending(x => x.FCreatedTime).ProjectTo<PermissionOutput>().ToListAsync();
            return outputs;
        }

        public async Task AddAsync(PermissionAddInput input, int createBy)
        {
            if (await _dbContext.Permission.AnyAsync(x =>
                        (!input.FFullName.IsNullOrWhiteSpace() && x.FFullName == input.FFullName.Trim()) ||
                        (!input.FUrl.IsNullOrWhiteSpace() && x.FUrl == input.FUrl.Trim())))
            {
                throw new BusinessException($"参数错误:当前数据库已经存在相同权限数据,请检查");
            }

            if (input.FParentId.HasValue)
            {
                if (!await _dbContext.Permission.AnyAsync(x => x.FId == input.FParentId.Value))
                {
                    throw new BusinessException($"参数错误:参数名 {nameof(input.FParentId)};参数值 {input.FParentId.Value} 找不到对应数据库数据");
                }
            }

            if (!input.FTopMenuKey.IsNullOrWhiteSpace())
            {
                if (await _dbContext.Permission.AnyAsync(x => x.FTopMenuKey == input.FTopMenuKey.Trim()))
                {
                    throw new BusinessException($"参数错误:当前数据库已经存在相同 {nameof(input.FTopMenuKey)}={input.FTopMenuKey.Trim()} 的数据,请检查");
                }
            }

            var permission = _mapper.Map<Permission>(input);
            permission.FCreatedBy = createBy;
            await _dbContext.Permission.AddAsync(permission);
            await _dbContext.SaveChangesAsync();
        }

        public async Task EditAsync(int id, string name, string areaName, string controllerName, string actionName, string fullName, string url, int? parentId, int sort, string remark, int updateBy, string icon, MenuType menuType, string topMenuKey,bool? isMultiAction)
        {
            var permission = await _dbContext.Permission.SingleOrDefaultAsync(x => x.FId == id);

            if (null == permission)
            {
                throw new BusinessException($"{nameof(id)}参数错误,数据库找不到对应数据");
            }

            if (parentId.HasValue)
            {
                if (!await _dbContext.Permission.AnyAsync(x => x.FId == parentId.Value))
                {
                    throw new BusinessException($"参数错误:参数名 {nameof(parentId)};参数值 {parentId.Value} 找不到对应数据库数据");
                }
            }

            if (await _dbContext.Permission.AnyAsync(x => x.FId != permission.FId &&
                    ((!fullName.IsNullOrEmpty() && x.FFullName == fullName) || (!url.IsNullOrEmpty() && x.FUrl == url))))
            {
                throw new BusinessException($"参数错误,当前数据库已经存在相同权限数据");
            }

            if (!topMenuKey.IsNullOrWhiteSpace())
            {
                if (await _dbContext.Permission.AnyAsync(x => x.FId != permission.FId && x.FTopMenuKey == topMenuKey.Trim()))
                {
                    throw new BusinessException($"参数错误:当前数据库已经存在相同 {nameof(topMenuKey)}={topMenuKey.Trim()} 的数据,请检查");
                }
            }

            permission.FName = name;
            permission.FAreaName = areaName;
            permission.FControllerName = controllerName;
            permission.FActionName = actionName;
            permission.FFullName = fullName;
            permission.FUrl = url;
            permission.FParentId = parentId;
            permission.FSort = sort;
            permission.FRemark = remark;
            permission.FIcon = icon;
            permission.FUpdatedBy = updateBy;
            permission.FUpdatedTime = DateTime.UtcNow;
            permission.FMenuType = menuType;
            permission.FTopMenuKey = topMenuKey;
            permission.FIsMultiAction = isMultiAction;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PermissionOutput> GetByIdAsync(int id)
        {
            var permission = await _dbContext.Permission.SingleOrDefaultAsync(x => x.FId == id);
            if (null == permission)
            {
                throw new BusinessException($"{nameof(id)}参数错误,数据库找不到对应数据");
            }
            return _mapper.Map<PermissionOutput>(permission);
        }
    }
}