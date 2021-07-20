using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using YQTrack.Backend.Email.Repository;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Email.DTO.Input;
using YQTrack.Core.Backend.Admin.Email.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Email.Service.Imp
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// 邮件数据库上下文
        /// </summary>
        private readonly EmailDbContext _emailDbContext;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="emailDbContext"></param>
        public EmailService(EmailDbContext emailDbContext)
        {
            _emailDbContext = emailDbContext;
        }

        /// <summary>
        /// 获取邮件发送记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<EmailRecordPageDataOutput> outputs, int total)> GetEmailRecordPageDataAsync(EmailRecordPageDataInput input)
        {
            var queryable = _emailDbContext.TSenderRecord
                .Where(x =>
                        x.FSubmitTime >= input.SubmitStartTime.Date &&
                        x.FSubmitTime < input.SubmitEndTime.AddDays(1).Date &&
                        x.FTo == input.To)
                .WhereIf(() => !string.IsNullOrWhiteSpace(input.From), x => x.FFrom == input.From);
            var count = await queryable.CountAsync();
            var outputs = await queryable.OrderByDescending(x => x.FCreateTime).ToPage(input.Page, input.Limit).ProjectTo<EmailRecordPageDataOutput>().ToListAsync();
            return (outputs, count);
        }

        /// <summary>
        /// 查询邮件投递详情
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DeliveryRecordDataOutput>> GetDeliveryRecordDataOutputAsync(string messageId)
        {
            var outputs = await _emailDbContext.TSenderReport
                .Where(x => x.FMessageId == messageId)
                .OrderByDescending(x => x.FCreateTime)
                .ProjectTo<DeliveryRecordDataOutput>()
                .ToListAsync();
            return outputs;
        }
    }
}