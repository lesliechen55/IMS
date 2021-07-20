using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;

namespace YQTrack.Core.Backend.Admin.User.DTO.Output
{
    public class UserDetailOutput
    {
        public long FuserId { get; set; }
        public string Femail { get; set; }
        public string FnickName { get; set; }
        public byte? FnodeId { get; set; }
        public byte? FdbNo { get; set; }
        public byte? FtableNo { get; set; }
        public SourceType Fsource { get; set; }
        public UserRoleType? FuserRole { get; set; }
        public DateTime? FlastSignIn { get; set; }
        public DateTime? FcreateTime { get; set; }

        /// <summary>
        /// 卖家信息
        /// </summary>
        public SellerInfoOutput SellerInfo { get; set; }

        /// <summary>
        /// 买家信息
        /// </summary>
        public List<MemberInfoOutput> ListMemberInfo { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public List<UserDeviceOutput> ListUserDevice { get; set; }

        /// <summary>
        /// 有效的(交易成功/退款的)最近5条交易记录
        /// </summary>
        public List<PaymentOutput> ListPayment { get; set; }
    }

    public class SellerInfoOutput
    {
        /// <summary>
        /// 当前有效的跟踪额度
        /// </summary>
        public int TrackServiceCount { set; get; }

        /// <summary>
        /// 当前剩余跟踪额度（计算字段）
        /// </summary>
        public int TrackRemainCount { set; get; }

        /// <summary>
        /// 当前有效的邮件额度
        /// </summary>
        public int EmailServiceCount { set; get; }

        /// <summary>
        /// 当前剩余邮件额度（计算字段）
        /// </summary>
        public int EmailRemainCount { set; get; }

        /// <summary>
        /// 用户路由数据，主要用于参数传递
        /// </summary>
        public UserRouteDto UserRoute { set; get; }
    }

    public class MemberInfoOutput
    {
        /// <summary>
        /// 会员级别
        /// </summary>
        public UserMemberLevel FmemberLevel { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? FstartTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? FexpiresTime { get; set; }
    }
    public class UserDeviceOutput
    {
        /// <summary>
        /// 设备语言
        /// </summary>
        public string Flanguage { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string FdeviceModel { get; set; }
        /// <summary>
        /// 操作系统类型
        /// </summary>
        public string Fostype { get; set; }
        /// <summary>
        /// 操作系统版本
        /// </summary>
        public string Fosversion { get; set; }
        /// <summary>
        /// App版本
        /// </summary>
        public string FappVersion { get; set; }
        /// <summary>
        /// 是否开启推送
        /// </summary>
        public bool? FisPush { get; set; }
        /// <summary>
        /// 推送令牌是否有效
        /// </summary>
        public bool? FisValid { get; set; }
        /// <summary>
        /// 最近访问时间
        /// </summary>
        public DateTime? FlastVisitTime { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public string FdeviceId { get; set; }

        /// <summary>
        /// 推送令牌
        /// </summary>
        public string FpushToken { get; set; }
    }
    public class PaymentOutput
    {
        public long FOrderId { get; set; }
        public CurrencyType FCurrencyType { get; set; }
        public PaymentProvider FProviderId { get; set; }
        public string FOrderName { get; set; }
        public decimal? FPaymentAmount { get; set; }
        public PaymentStatus FPaymentStatus { get; set; }
        public DateTime FCreateAt { get; set; }
        public DateTime FUpdateAt { get; set; }
    }
}