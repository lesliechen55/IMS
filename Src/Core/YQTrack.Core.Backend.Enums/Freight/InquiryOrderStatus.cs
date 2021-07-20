using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.Freight
{
    public enum InquiryOrderStatus
    {
        /// <summary>
        /// 已发布
        /// </summary>
        [Description("已发布")]
        Published = 0,

        /// <summary>
        /// 已经截止
        /// </summary>
        [Description("已经截止")]
        Stopped = 1,

        /// <summary>
        /// 交易成功
        /// </summary>
        [Description("交易成功")]
        BusinessSuccess = 2,

        /// <summary>
        /// 交易失败
        /// </summary>
        [Description("交易失败")]
        BusinessFailed = 3,

        /// <summary>
        /// 管理员下架
        /// </summary>
        [Description("管理员下架")]
        MangerReject = 4,

        /// <summary>
        /// 竞价单撤销竞价(特别说明:竞价单撤销竞价是竞价人的行为,应该不属于询价单的枚举状态,放进这里是为了前端资源文件统一,故而为之)
        /// </summary>
        [Description("竞价单撤销竞价")]
        QuoteCanceled = 5
    }
}