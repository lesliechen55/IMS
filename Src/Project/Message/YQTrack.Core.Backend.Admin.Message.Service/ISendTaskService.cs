using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Core.Enum;
using YQTrack.Core.Backend.Admin.Data.Interceptor;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Message.Service
{
    public interface ISendTaskService : IScopeService
    {
        /// <summary>
        /// 获取发送任务分页列表
        /// </summary>
        /// <param name="input">发送任务列表搜索条件</param>
        /// <returns></returns>
        Task<(IEnumerable<SendTaskPageDataOutput> outputs, int total)> GetPageDataAsync(SendTaskPageDataInput input);

        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SendTaskEditOutput> GetByIdAsync(long id);

        /// <summary>
        /// 添加发送任务
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorName">操作人</param>
        /// <returns></returns>
        [OperationTrace(desc: "添加发送任务信息", type: OperationType.Add)]
        Task<bool> AddAsync(SendTaskEditInput input, string operatorName);

        /// <summary>
        /// 修改发送任务
        /// </summary>
        /// <param name="input"></param>
        /// <param name="operatorName">操作人</param>
        /// <returns></returns>
        [OperationTrace(desc: "修改发送任务信息", type: OperationType.Edit)]
        Task<bool> EditAsync(SendTaskEditInput input, string operatorName);

        /// <summary>
        /// 测试发送语言模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [OperationTrace(desc: "测试发送语言模板", type: OperationType.Edit)]
        Task<bool> SendTemplateTestAsync(SendTemplateTestInput input);
    }
}
