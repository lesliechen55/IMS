using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Data;
using YQTrack.Core.Backend.Admin.Data.Entity;
using YQTrack.Core.Backend.Admin.DTO;
using YQTrack.Core.Backend.Admin.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Service.Imp
{
    public class ESDashboardService : IESDashboardService
    {
        private readonly AdminDbContext _dbContext;
        private readonly IMapper _mapper;

        public ESDashboardService(AdminDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ESDashboardDetailOutput> GetByIdAsync(int id)
        {
            ESDashboardDetailOutput output = new ESDashboardDetailOutput();
            output.ESDashboard = await _dbContext.ESDashboard.Where(x => x.FPermissionId == id).ProjectTo<ESDashboardDto>().SingleOrDefaultAsync() ?? new ESDashboardDto() { FPermissionId = id };
            output.TimeRanges = await _dbContext.ESField.Where(w => w.FCategory == "TimeRange").ProjectTo<ESFieldOutput>().ToListAsync();
            output.Categories = await _dbContext.ESField.Where(w => w.FCategory != "TimeRange" && w.FCategory.IsNotNullOrWhiteSpace()).Select(s => s.FCategory).Distinct().OrderBy(o => o).ToArrayAsync();
            return output;
        }

        public async Task SetAsync(ESDashboardDto input)
        {
            var eSDashboard = await _dbContext.ESDashboard.SingleOrDefaultAsync(x => x.FPermissionId == input.FPermissionId);
            if (null == eSDashboard)
            {
                eSDashboard = _mapper.Map<ESDashboard>(input);
                await _dbContext.ESDashboard.AddAsync(eSDashboard);
            }
            else
            {
                eSDashboard.FDashboardSrc = input.FDashboardSrc;
                eSDashboard.FUsername = input.FUsername;
                eSDashboard.FPassword = input.FPassword;
                eSDashboard.FMaxDateRange = input.FMaxDateRange;
                eSDashboard.FFieldsConfig = JsonHelper.ToJson(input.FFieldsConfig);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ESFieldOutput>> GetDataByCategoryAsync(string category)
        {
            return await _dbContext.ESField.Where(w => w.FCategory == category).ProjectTo<ESFieldOutput>().OrderBy(o => o.FSort).ToListAsync();
        }

        public async Task<ESDashboardDetailOutput> GetByPermissionCodeAsync(string permissionCode)
        {
            int permissionId = await _dbContext.Permission.Where(w => w.FFullName == permissionCode).Select(s => s.FId).FirstOrDefaultAsync();
            if (permissionId <= 0)
            {
                throw new BusinessException($"{nameof(permissionCode)}:{permissionCode}不正确，没有相关权限数据");
            }
            ESDashboardDetailOutput output = new ESDashboardDetailOutput();
            var eSDashboard = await _dbContext.ESDashboard.Where(x => x.FPermissionId == permissionId).ProjectTo<ESDashboardDto>().SingleOrDefaultAsync();
            if (eSDashboard == null)
            {
                throw new BusinessException("没有配置相关ES Dashboard数据");
            }
            output.ESDashboard = eSDashboard;

            output.TimeRanges = await _dbContext.ESField.Where(w => w.FCategory == "TimeRange" && w.FSort <= eSDashboard.FMaxDateRange).ProjectTo<ESFieldOutput>().OrderBy(o => o.FSort).ToListAsync();
            return output;
        }
    }
}