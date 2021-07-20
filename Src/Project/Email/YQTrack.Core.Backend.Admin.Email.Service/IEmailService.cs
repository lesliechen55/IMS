using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Email.DTO.Input;
using YQTrack.Core.Backend.Admin.Email.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Email.Service
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    public interface IEmailService : IScopeService
    {
        /// <summary>
        /// 获取邮件发送记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<(IEnumerable<EmailRecordPageDataOutput> outputs, int total)> GetEmailRecordPageDataAsync(
            EmailRecordPageDataInput input);

        /// <summary>
        /// 查询邮件投递详情
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        Task<IEnumerable<DeliveryRecordDataOutput>> GetDeliveryRecordDataOutputAsync(string messageId);
    }
}