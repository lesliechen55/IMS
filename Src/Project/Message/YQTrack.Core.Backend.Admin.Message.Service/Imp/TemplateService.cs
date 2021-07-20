using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.Core.Redis;
using YQTrack.Core.Backend.Admin.Message.Data;
using YQTrack.Core.Backend.Admin.Message.Data.Models;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Message.Service.Imp
{
    public class TemplateService : ITemplateService
    {
        private readonly MessageDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly CacheHelper _cacheHelper;

        public TemplateService(MessageDbContext dbContext, IMapper mapper, CacheHelper cacheHelper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cacheHelper = cacheHelper;
        }
        /// <summary>
        /// 获取语言模板分页列表
        /// </summary>
        /// <param name="input">语言模板列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<TemplatePageDataOutput> outputs, int total)> GetPageDataAsync(TemplatePageDataInput input)
        {

            var queryable = _dbContext.Ttemplate
                .GroupJoin(_dbContext.TtemplateType, x => x.FtemplateTypeId, y => y.FtemplateTypeId, (x, y) => new { Template = x, TemplateType = y })
                .SelectMany(xy => xy.TemplateType.DefaultIfEmpty(), (x, y) => new { Template = x.Template, TemplateType = y })
                .GroupJoin(_dbContext.Tproject, x => x.TemplateType.FprojectId, y => y.FprojectId, (x, y) => new { Template = x.Template, TemplateType = x.TemplateType, Project = y })
                .SelectMany(xy => xy.Project.DefaultIfEmpty(), (x, y) => new { Template = x.Template, TemplateType = x.TemplateType, Project = y })
                .Where(x => x.Template.FisDel == 0 && x.Template.FtemplateTypeId == input.TemplateTypeId)
                .WhereIf(() => !string.IsNullOrWhiteSpace(input.Language), x => x.Template.Flanguage == input.Language);

            var count = await queryable.CountAsync();
            Dictionary<string, string> langs = LanguageHelper.GetLanguageList();
            var outputs = await queryable
                .OrderBy(x => x.Template.Flanguage)
                .Select(s => new TemplatePageDataOutput
                {
                    FTemplateId = s.Template.FtemplateId,
                    FChannel = (ChannelSend)s.TemplateType.Fchannel,
                    FLanguage = langs.SingleOrDefault(sd => sd.Key == s.Template.Flanguage).Value,
                    FProjectName = s.Project.FprojectName,
                    FTemplateName = s.TemplateType.FtemplateName,
                    FTemplateTitle = s.Template.FtemplateTitle
                }).ToPage(input.Page, input.Limit)
                .ToListAsync();

            return (outputs, count);
        }

        /// <summary>
        /// 添加语言模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(TemplateEditInput input)
        {
            Ttemplate model = _mapper.Map<Ttemplate>(input);
            model.FtemplateId = IdHelper.GetGenerateId();
            model.FisDel = 0;
            await _dbContext.Ttemplate.AddAsync(model);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 添加语言模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(long id)
        {
            Ttemplate model = _cacheHelper.GetCache<Ttemplate>(id.ToString(CultureInfo.InvariantCulture));
            if (model == null)
            {
                throw new BusinessException("缓存数据已丢失，请重新导入。");
            }
            if (await _dbContext.Ttemplate.AnyAsync(x => x.FtemplateId == id))
            {
                _dbContext.Ttemplate.Update(model);
            }
            else
            {
                await _dbContext.Ttemplate.AddAsync(model);
            }
            _cacheHelper.RemoveAllCache(id.ToString(CultureInfo.InvariantCulture));
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 删除语言模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(long id)
        {
            Ttemplate model = await _dbContext.Ttemplate.FirstOrDefaultAsync(x => x.FtemplateId == id);
            if (model == null)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(model)}数据不存在");
            }
            if (model.Flanguage.Equals("en", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new BusinessException("英文模板不能删除");
            }
            model.FisDel = 1;
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 删除语言模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RealDeleteAsync(long id)
        {
            _cacheHelper.RemoveAllCache(id.ToString(CultureInfo.InvariantCulture));
            await Task.CompletedTask;
        }

    }
}
