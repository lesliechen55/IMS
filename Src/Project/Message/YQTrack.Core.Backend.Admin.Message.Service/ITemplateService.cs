using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Message.Service
{
    public interface ITemplateService : IScopeService
    {
        /// <summary>
        /// 获取语言模板分页列表
        /// </summary>
        /// <param name="input">模板列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<TemplatePageDataOutput> outputs, int total)> GetPageDataAsync(TemplatePageDataInput input);
        /// <summary>
        /// 添加语言模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationTrace(desc: "添加语言模板信息", type: OperationType.Add)]
        Task<bool> AddAsync(TemplateEditInput input);
        /// <summary>
        /// 添加语言模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationTrace(desc: "导入语言模板信息", type: OperationType.Add)]
        Task<bool> AddAsync(long id);
        /// <summary>
        /// 删除语言模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationTrace(desc: "删除语言模板信息", type: OperationType.Delete)]
        Task<bool> DeleteAsync(long id);
        /// <summary>
        /// 删除语言模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RealDeleteAsync(long id);
    }
}
