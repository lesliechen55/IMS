using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Backend.Message.Model.Models;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.Core.Redis;
using YQTrack.Core.Backend.Admin.Message.Data;
using YQTrack.Core.Backend.Admin.Message.Data.Models;
using YQTrack.Core.Backend.Admin.Message.DTO;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;
using YQTrack.Core.Backend.Admin.Message.Service.RemoteApi;
using YQTrack.Standard.Common.Utils;

namespace YQTrack.Core.Backend.Admin.Message.Service.Imp
{
    public class TemplateTypeService : ITemplateTypeService
    {
        private readonly MessageDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly MessageService _msgService;
        private readonly CacheHelper _cacheHelper;

        public TemplateTypeService(MessageDbContext dbContext, IMapper mapper, MessageService msgService, CacheHelper cacheHelper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _cacheHelper = cacheHelper;
            _msgService = msgService;
        }
        /// <summary>
        /// 获取基础模板分页列表
        /// </summary>
        /// <param name="input">基础模板列表搜索条件</param>
        /// <returns></returns>
        public async Task<(IEnumerable<TemplateTypePageDataOutput> outputs, int total)> GetPageDataAsync(TemplateTypePageDataInput input)
        {
            var queryable = _dbContext.TtemplateType
                .GroupJoin(_dbContext.Tproject, x => x.FprojectId, y => y.FprojectId, (x, y) => new { x, y })
                .SelectMany(xy => xy.y.DefaultIfEmpty(), (x, y) => new { a = x.x, b = y })
                .WhereIf(() => input.ProjectId.HasValue, x => x.a.FprojectId == input.ProjectId.Value)
                .WhereIf(() => input.ChannelId.HasValue, x => x.a.Fchannel == (int)input.ChannelId.Value)
                .WhereIf(() => !string.IsNullOrWhiteSpace(input.TemplateName), x => x.a.FtemplateName.Contains(input.TemplateName));

            var count = await queryable.CountAsync();

            var outputs = await queryable
                .OrderByDescending(x => x.a.FupdateTime)
                .Select(s => new TemplateTypePageDataOutput
                {
                    FChannel = (ChannelSend)s.a.Fchannel,
                    FCreateTime = s.a.FcreateTime,
                    FDataJson = s.a.FdataJson,
                    FEnable = s.a.Fenable,
                    FIsRendering = s.a.FisRendering,
                    FProjectName = s.b.FprojectName,
                    FTemplateCode = s.a.FtemplateCode,
                    FTemplateDescribe = s.a.FtemplateDescribe,
                    FTemplateName = s.a.FtemplateName,
                    FTemplateTypeId = s.a.FtemplateTypeId,
                    FUpdateTime = s.a.FupdateTime
                }).ToPage(input.Page, input.Limit)
                .ToListAsync();

            return (outputs, count);
        }

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TemplateTypeEditOutput> GetByIdAsync(long id)
        {
            var model = await _dbContext.TtemplateType.SingleOrDefaultAsync(x => x.FtemplateTypeId == id);
            if (null == model)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(model)}数据不存在");
            }
            var output = _mapper.Map<TemplateTypeEditOutput>(model);
            return output;
        }
        /// <summary>
        /// 添加基础模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(TemplateTypeEditInput input)
        {
            TtemplateType model = _mapper.Map<TtemplateType>(input);
            model.FtemplateTypeId = IdHelper.GetGenerateId();
            model.Fenable = 1;
            model.FcreateTime = DateTime.UtcNow;
            model.FupdateTime = DateTime.UtcNow;
            model.FtemplateCode = (int)MessageTemplateType.CommonTemplate;

            AnalysisToData(model.FtemplateTitle, true);
            AnalysisToData(model.FtemplateBody);
            await _dbContext.TtemplateType.AddAsync(model);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 修改基础模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> EditAsync(TemplateTypeEditInput input)
        {
            TtemplateType model = await _dbContext.TtemplateType.SingleOrDefaultAsync(x => x.FtemplateTypeId == input.TemplateTypeId);
            if (null == model)
            {
                throw new BusinessException($"{nameof(input.TemplateTypeId)}参数错误,{nameof(model)}数据不存在");
            }
            _cacheHelper.RemoveAllCache(Convert.ToString(model.FtemplateTypeId, CultureInfo.InvariantCulture));
            _cacheHelper.RemoveAllCache(Convert.ToString(model.FtemplateCode, CultureInfo.InvariantCulture));
            model.Fchannel = (int)input.Channel;
            //model.Fenable = 1;
            model.FdataJson = input.DataJson;
            model.FprojectId = input.ProjectId;
            model.FtemplateDescribe = input.TemplateDescribe;
            model.FtemplateName = input.TemplateName;
            model.FtemplateTitle = input.TemplateTitle;
            model.FtemplateBody = input.TemplateBody;
            model.FupdateTime = DateTime.UtcNow;

            AnalysisToData(model.FtemplateTitle, true);
            AnalysisToData(model.FtemplateBody);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 导出模板翻译文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(byte[] fileContent, string fileName)> ExportAsync(long id)
        {
            TtemplateType model = await _dbContext.TtemplateType.SingleOrDefaultAsync(x => x.FtemplateTypeId == id);
            if (null == model)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(model)}数据不存在");
            }

            TemplateJsonData data = new TemplateJsonData();
            data.TemplateCode = model.FtemplateCode.Value;
            data.TemplateTypeId = model.FtemplateTypeId.ToString();

            //解析模板数据 必须先解析后渲染，因为渲染后有的标签可能没了。
            data.TitleDict = AnalysisToData(model.FtemplateTitle, true);
            data.BodyDict = AnalysisToData(model.FtemplateBody);

            //渲染动态内容
            string htmlTitle = await _msgService.RenderAsync(GetRazorData(model, model.FtemplateTitle, true));
            string htmlContent = await _msgService.RenderAsync(GetRazorData(model, model.FtemplateBody));

            byte[] fileContent = GetFileContent(data, htmlTitle, htmlContent);
            return (fileContent, $"{data.TemplateCode}.html");
        }

        /// <summary>
        /// 获取渲染数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="content"></param>
        /// <param name="language"></param>
        /// <param name="isCanEmpty"></param>
        /// <returns></returns>
        private static TemplateRenderData GetRazorData(TtemplateType model, string content, bool isCanEmpty = false, string language = "en")
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                if (isCanEmpty)
                {
                    return null;
                }
                throw new BusinessException("缺少基础模板数据");
            }
            var razorData = new TemplateRenderData()
            {
                FTemplateId = model.FtemplateTypeId,
                TemplateContent = content,
                ProducerData = null,
                Language = language
            };
            //样例数据不存在
            if (string.IsNullOrWhiteSpace(model.FdataJson))
            {
                return razorData;
            }
            MessageModel msg = null;
            try
            {
                msg = SerializeExtend.MyDeserializeFromJson<MessageModel>(model.FdataJson);
            }
            catch (Exception)
            {
                throw new BusinessException("基础模板样例数据格式不正确");
            }
            razorData.DataJson = msg == null ? "" : msg.TemplateData;
            razorData.MessageData = msg == null ? "" : msg.TemplateData;
            razorData.UserInfo = msg == null ? new UserInfoExt() : msg.UserId;
            return razorData;
        }
        /// <summary>
        /// 词条正则
        /// </summary>
        private static readonly string strPattern = "<dict\\s*id\\s*=\\s*\\\\?[\"\'](__\\d+)\\\\?[\"\']\\s*>([\\s\\S]*?)</dict>";
        /// <summary>
        /// 占位符标记正则
        /// </summary>
        private static readonly string strPlaceTagPattern = "<tag\\s*id\\s*=\\s*\\\\?[\"\']_(\\d+)_\\\\?[\"\']\\s*>([\\s\\S]*?)</tag>";
        /// <summary>
        /// 占位符正则
        /// </summary>
        private static readonly string strPlaceHolderPattern = "{(\\d+)}";
        /// <summary>
        /// 解析并验证模板数据
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <param name="isCanEmpty">内容或词条是否可为空</param>
        private static Collection<Dict> AnalysisToData(string htmlContent, bool isCanEmpty = false)
        {
            //词条集合
            Collection<Dict> data = new Collection<Dict>();
            if (string.IsNullOrWhiteSpace(htmlContent))
            {
                if (isCanEmpty)
                {
                    return data;
                }
                throw new BusinessException("缺少基础模板数据");
            }
            //解析语言条目
            Regex regex = new Regex(strPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex regexPlace = new Regex(strPlaceTagPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (regex.IsMatch(htmlContent))
            {
                MatchCollection matchCollection = regex.Matches(htmlContent);
                foreach (Match match in matchCollection)
                {
                    Dict d = new Dict
                    {
                        Id = match.Groups[1].Value,
                        Source = match.Groups[2].Value,
                        Target = ""
                    };
                    //占位符替换
                    if (regexPlace.IsMatch(d.Source))
                    {
                        MatchCollection matchCollectionPlace = regexPlace.Matches(d.Source);
                        foreach (Match m in matchCollectionPlace)
                        { 
                            d.Source = d.Source.Replace(m.Groups[0].Value, $"{{{m.Groups[1].Value}}}");
                        }
                    }
                    data.Add(d);
                }
            }
            //验证语言条目格式
            ValidTemplateJsonData(data, isCanEmpty);
            return data;
        }

        /// <summary>
        /// 获取基础模板翻译文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="htmlTitle"></param>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        private static byte[] GetFileContent(TemplateJsonData data, string htmlTitle, string htmlContent)
        {
            //读取模板翻译文件
            string tempPath = $"{Directory.GetCurrentDirectory()}/file/template.html";
            string tempContent = FileHelper.ReadFile(tempPath);
            if (string.IsNullOrWhiteSpace(tempContent))
            {
                throw new BusinessException("未能获取到模板翻译文件");
            }
            //替换模板数据
            string jsonContent = JsonHelper.ToJson(data);
            tempContent = tempContent
                .Replace("<!--template title area-->", htmlTitle)
                .Replace("<!--template content area-->", htmlContent)
                .Replace("//template json area", jsonContent);
            //string path = $"{Directory.GetCurrentDirectory()}/file/{data.TemplateCode}/{data.TemplateCode}";
            //string htmlPath = Path.ChangeExtension(path, "html");
            //FileHelper.String2File(htmlContent, htmlPath);;

            ////压缩成一个zip文件
            //string zipPath = $"{Directory.GetCurrentDirectory()}/file/{data.TemplateCode}";
            //string zipFile = Path.ChangeExtension($"{zipPath}_en", "zip");
            //ZipFile.CreateFromDirectory(zipPath, zipFile);
            byte[] fileCount = Encoding.UTF8.GetBytes(tempContent);
            return fileCount;
        }

        /// <summary>
        /// 验证语言条目格式
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isCanEmpty">词条是否可为空</param>
        private static void ValidTemplateJsonData(Collection<Dict> data, bool isCanEmpty = false)
        {
            //验证数据
            if (!data.Any())
            {
                if (isCanEmpty)
                {
                    return;
                }
                throw new BusinessException("未能获取到语言条目数据");
            }
            //重复语言条目ID集合
            List<string> list = data.GroupBy(g => g.Id).Where(w => w.Count() > 1).Select(s => s.Key).ToList();
            if (list.Any())
            {
                throw new BusinessException($"语言条目ID重复：{string.Join(',', list)}");
            }
            //占位符ID集合
            List<string> placeHolders = new List<string>();
            foreach (var item in data)
            {
                Regex regexPlaceHolder = new Regex(strPlaceHolderPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if (regexPlaceHolder.IsMatch(item.Source))
                {
                    MatchCollection matchCollection = regexPlaceHolder.Matches(item.Source);
                    foreach (Match m in matchCollection)
                    {
                        placeHolders.Add(m.Groups[1].Value);
                    }
                }
            }
            //验证占位符ID是否重复
            placeHolders = placeHolders.GroupBy(g => g).Where(w => w.Count() > 1).Select(s => $"_{s.Key}_").ToList();
            if (placeHolders.Any())
            {
                throw new BusinessException($"占位符ID重复：{string.Join(',', placeHolders)}");
            }
        }

        /// <summary>
        /// 导入词条，预览模板
        /// </summary>
        /// <param name="jsonData">词条数据</param>
        /// <param name="language">语言</param>
        /// <returns>预览、保存数据</returns>
        public async Task<ImportShowOutput> ImportAsync(string jsonData, string language)
        {
            TemplateJsonData data = SerializeExtend.MyDeserializeFromJson<TemplateJsonData>(jsonData);
            TtemplateType model = await _dbContext.TtemplateType.AsNoTracking().SingleOrDefaultAsync(x => x.FtemplateTypeId == long.Parse(data.TemplateTypeId));
            if (null == model)
            {
                throw new BusinessException($"{nameof(data.TemplateTypeId)}参数错误,{nameof(model)}数据不存在");
            }
            Ttemplate tempModel = await _dbContext.Ttemplate.FirstOrDefaultAsync(a => a.FtemplateTypeId == model.FtemplateTypeId && a.Flanguage == language && a.FisDel == 0);
            if (tempModel == null)
            {
                tempModel = new Ttemplate
                {
                    FtemplateId = IdHelper.GetGenerateId(),
                    Flanguage = language,
                    FisDel = 0,
                    FtemplateTypeId = model.FtemplateTypeId
                };
            }
            tempModel.FtemplateTitle = GenerateTemplate(model.FtemplateTitle, data.TitleDict, true);
            tempModel.FtemplateBody = GenerateTemplate(model.FtemplateBody, data.BodyDict);
            tempModel.FTemplateData = jsonData;

            _cacheHelper.SetCache(tempModel.FtemplateId.ToString(CultureInfo.InvariantCulture), tempModel);

            //if (await _dbContext.SaveChangesAsync() == 0)
            //{
            //    throw new BusinessException($"{language}语言模板保存失败");
            //}

            ImportShowOutput output = new ImportShowOutput();
            output.TemplateTypeId = model.FtemplateTypeId.ToString();
            output.TemplateId = tempModel.FtemplateId.ToString();
            output.TemplateName = model.FtemplateName;
            return output;
        }

        /// <summary>
        /// 替换语言条目，生成语言模板（去除span解析标识）
        /// </summary>
        /// <param name="htmlContent">模板数据</param>
        /// <param name="jsonData">词条数据</param>
        /// <param name="isCanEmpty">内容或词条是否可为空</param>
        private static string GenerateTemplate(string htmlContent, Collection<Dict> dictData, bool isCanEmpty = false)
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
            {
                if (isCanEmpty)
                {
                    return string.Empty;
                }
                throw new BusinessException("基础模板数据不完整");
            }

            //验证语言条目格式
            ValidTemplateJsonData(dictData);

            string data = htmlContent;
            //替换语言条目
            Regex regex = new Regex(strPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (regex.IsMatch(htmlContent))
            {
                MatchCollection matchCollection = regex.Matches(htmlContent);
                Regex regexPlaceTag = new Regex(strPlaceTagPattern, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                foreach (Match match in matchCollection)
                {
                    Dict dict = dictData.SingleOrDefault(s => s.Id.Equals(match.Groups[1].Value, StringComparison.InvariantCulture));
                    if (dict == null)
                    {
                        throw new BusinessException($"未能获取到语言条目数据:id={match.Groups[1].Value}");
                    }
                    //占位符标记内容替换词条中的占位符
                    string source = match.Groups[0].Value;
                    if (regexPlaceTag.IsMatch(source))
                    {
                        MatchCollection matchCollectionPlaceTag = regexPlaceTag.Matches(source);
                        foreach (Match matchTag in matchCollectionPlaceTag)
                        {
                            string strPlaceHolder = $"\\{{{matchTag.Groups[1].Value}\\}}";
                            Regex regexPlaceHolder = new Regex(strPlaceHolder, RegexOptions.IgnoreCase | RegexOptions.Multiline);
                            if (regexPlaceHolder.IsMatch(dict.Target))
                            {
                                //string matchValue = regexPlaceHolder.Match(dict.Target).Value;
                                dict.Target = regexPlaceHolder.Replace(dict.Target, matchTag.Groups[2].Value);
                            }
                            else
                            {
                                throw new BusinessException($"未能检测到占位符:{{{matchTag.Groups[1].Value}}}");
                            }
                        }
                    }
                    data = data.Replace(source, dict.Target);
                }
            }
            return data;
        }

        /// <summary>
        /// 根据基础模板内容及语言词条内容批量更新语言模板
        /// </summary>
        /// <param name="id">基础模板ID</param>
        /// <returns></returns>
        public async Task<bool> UpdateTemplateAsync(long id)
        {
            TtemplateType model = await _dbContext.TtemplateType.SingleOrDefaultAsync(x => x.FtemplateTypeId == id);
            if (null == model)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(model)}数据不存在");
            }
            List<Ttemplate> list = _dbContext.Ttemplate.Where(w => w.FtemplateTypeId == model.FtemplateTypeId && w.FisDel == 0).ToList();
            TemplateJsonData data = null;
            list.ForEach(f =>
            {
                if (string.IsNullOrWhiteSpace(f.FTemplateData))
                {
                    throw new BusinessException($"语言：{f.Flanguage} 词条数据不存在，请导入词条后再操作");
                }
                _cacheHelper.RemoveAllCache($"{f.FtemplateTypeId.ToString(CultureInfo.InvariantCulture)}_{f.Flanguage}");

                data = SerializeExtend.MyDeserializeFromJson<TemplateJsonData>(f.FTemplateData);
                f.FtemplateTitle = GenerateTemplate(model.FtemplateTitle, data.TitleDict, true);
                f.FtemplateBody = GenerateTemplate(model.FtemplateBody, data.BodyDict);
            });
            _dbContext.Ttemplate.UpdateRange(list);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 模板预览
        /// </summary>
        /// <param name="id">模板ID</param>
        /// <returns></returns>
        public async Task<TemplateShowOutput> PreviewAsync(long id)
        {
            TtemplateType model = await _dbContext.TtemplateType.AsNoTracking().SingleOrDefaultAsync(x => x.FtemplateTypeId == id);
            if (null == model)
            {
                throw new BusinessException($"{nameof(id)}参数错误,{nameof(model)}数据不存在");
            }
            TemplateShowOutput output = new TemplateShowOutput();
            output.TemplateId = model.FtemplateTypeId.ToString();
            //渲染动态内容
            output.TemplateTitle = await _msgService.RenderAsync(GetRazorData(model, model.FtemplateTitle, true));
            output.TemplateBody = await _msgService.RenderAsync(GetRazorData(model, model.FtemplateBody));
            return output;
        }

        /// <summary>
        /// 模板预览
        /// </summary>
        /// <param name="id">语言模板ID</param>
        /// <param name="loadInCache">是否从缓存加载</param>
        /// <returns></returns>
        public async Task<TemplateShowOutput> TemplatePreviewAsync(long id, bool loadInCache = false)
        {
            Ttemplate tempModel = null;
            if (loadInCache)
            {
                tempModel = _cacheHelper.GetCache<Ttemplate>(id.ToString(CultureInfo.InvariantCulture));
                if (tempModel == null)
                {
                    throw new BusinessException("缓存数据已丢失，请重新导入。");
                }
            }
            else
            {
                tempModel = await _dbContext.Ttemplate.AsNoTracking().SingleOrDefaultAsync(x => x.FtemplateId == id);
                if (null == tempModel)
                {
                    throw new BusinessException($"{nameof(id)}参数错误,{typeof(Ttemplate).Name}数据不存在");
                }
            }
            TtemplateType model = await _dbContext.TtemplateType.AsNoTracking().SingleOrDefaultAsync(x => x.FtemplateTypeId == tempModel.FtemplateTypeId);
            if (null == model)
            {
                throw new BusinessException($"{nameof(tempModel.FtemplateTypeId)}参数错误,{typeof(Ttemplate).Name}数据不存在");
            }
            TemplateShowOutput output = new TemplateShowOutput();
            output.TemplateId = tempModel.FtemplateId.ToString();
            //渲染动态内容
            output.TemplateTitle = await _msgService.RenderAsync(GetRazorData(model, tempModel.FtemplateTitle, true, tempModel.Flanguage));
            output.TemplateBody = await _msgService.RenderAsync(GetRazorData(model, tempModel.FtemplateBody, false, tempModel.Flanguage));
            return output;
        }

    }
}
