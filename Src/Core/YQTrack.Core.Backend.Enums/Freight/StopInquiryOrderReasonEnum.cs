namespace YQTrack.Core.Backend.Enums.Freight
{
    public enum StopInquiryOrderReasonEnum
    {
        /// <summary>
        /// 其他原因
        /// </summary>
        Other = 20,

        /// <summary>
        /// 已经找到合适的运输商
        /// </summary>
        AlreadyFoundFitCarrier = 21,

        /// <summary>
        /// 取消寄件
        /// </summary>
        CancelDelivery = 22,

        /// <summary>
        /// 没有合适运输商着急寄件
        /// </summary>
        NotFitCarrierHurryToDelivery = 23,


        /// <summary>
        /// 当前询价单没有任何竞价信息,系统定期识别为交易失败(前端不用渲染)
        /// </summary>
        AutomaticBusinessFailedByNotAnyQuote=10,

        /// <summary>
        /// 当前询价单有至少一个竞价信息,询价发布人未选择任何报价,系统定期识别为交易失败(前端不用渲染)
        /// </summary>
        AutomaticBusinessFailedByNotChooseAnyQuote = 11,

        /// <summary>
        /// 管理员下架(不渲染到前端页面只用做逻辑判断)(前端不用渲染)
        /// </summary>
        ManagerReject = 12
    }
}