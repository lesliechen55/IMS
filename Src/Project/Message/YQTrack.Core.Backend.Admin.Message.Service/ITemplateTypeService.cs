using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Message.Service
{
    public interface ITemplateTypeService: IScopeService
    {
        /// <summary>
        /// 获取基础模板分页列表
        /// </summary>
        /// <param name="input">基础模板列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<TemplateTypePageDataOutput> outputs, int total)> GetPageDataAsync(TemplateTypePageDataInput input);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TemplateTypeEditOutput> GetByIdAsync(long id);

        /// <summary>
        /// 添加基础模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationTrace(desc: "添加基础模板信息", type: OperationType.Add)]
        Task<bool> AddAsync(TemplateTypeEditInput input);

        /// <summary>
        /// 修改基础模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationTrace(desc: "修改基础模板信息", type: OperationType.Edit)]
        Task<bool> EditAsync(TemplateTypeEditInput input);

        /// <summary>
        /// 导出基础模板、语言条目（压缩文件）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(byte[] fileContent, string fileName)> ExportAsync(long id);

        /// <summary>
        /// 导入词条，预览模板
        /// </summary>
        /// <param name="jsonData">词条数据</param>
        /// <param name="language">语言</param>
        /// <returns>预览、保存数据</returns>
        Task<ImportShowOutput> ImportAsync(string jsonData, string language);

        /// <summary>
        /// 根据基础模板内容及语言词条内容批量更新语言模板
        /// </summary>
        /// <param name="id">基础模板ID</param>
        /// <returns></returns>
        [OperationTrace(desc: "批量更新语言模板信息", type: OperationType.Edit)]
        Task<bool> UpdateTemplateAsync(long id);

        /// <summary>
        /// 模板预览
        /// </summary>
        /// <param name="id">模板ID</param>
        /// <returns></returns>
        Task<TemplateShowOutput> PreviewAsync(long id);

        /// <summary>
        /// 模板预览
        /// </summary>
        /// <param name="id">语言模板ID</param>
        /// <param name="loadInCache">是否从缓存加载</param>
        /// <returns></returns>
        Task<TemplateShowOutput> TemplatePreviewAsync(long id, bool loadInCache = false);
    }
}
