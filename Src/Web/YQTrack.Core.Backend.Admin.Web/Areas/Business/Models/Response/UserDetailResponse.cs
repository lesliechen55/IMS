using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response
{
    public class UserDetailResponse
    {
        public long UserId { get; set; }

        public string Gid
        {
            get
            {
                return YQTrack.Utility.UserIdExtend.GetMaskUserIdByUserId(UserId);
            }
        }
        public string Email { get; set; }
        public string NickName { get; set; }
        public byte NodeId { get; set; }
        public byte DbNo { get; set; }
        public byte TableNo { get; set; }
        public string UserRole { get; set; }
        public string Source { get; set; }
        public DateTime? LastSignIn { get; set; }
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 卖家信息
        /// </summary>
        public SellerInfoResponse SellerInfo { get; set; }

        /// <summary>
        /// 买家信息
        /// </summary>
        public List<MemberInfoResponse> ListMemberInfo { get; set; }

        /// <summary>
        /// 设备信息
        /// </summary>
        public List<UserDeviceResponse> ListUserDevice { get; set; }

        /// <summary>
        /// 有效的(交易成功/退款的)最近5条交易记录
        /// </summary>
        public List<PaymentResponse> ListPayment { get; set; }
    }

    public class SellerInfoResponse
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
        public string UserRoute { set; get; }
    }

    public class MemberInfoResponse
    {
        /// <summary>
        /// 会员级别
        /// </summary>
        public UserMemberLevel MemberLevel { get; set; }
        /// <summary>
        /// 生效时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime? ExpiresTime { get; set; }
    }
    public class UserDeviceResponse
    {
        /// <summary>
        /// 设备语言
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// 型号
        /// </summary>
        public string DeviceModel { get; set; }
        /// <summary>
        /// 操作系统类型
        /// </summary>
        public string Ostype { get; set; }
        /// <summary>
        /// 操作系统版本
        /// </summary>
        public string Osversion { get; set; }
        /// <summary>
        /// App版本
        /// </summary>
        public string AppVersion { get; set; }
        /// <summary>
        /// 是否开启推送
        /// </summary>
        public bool IsPush { get; set; }
        /// <summary>
        /// 推送令牌是否有效
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 最近访问时间
        /// </summary>
        public DateTime? LastVisitTime { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public string DeviceId { get; set; }

        /// <summary>
        /// 推送令牌
        /// </summary>
        public string PushToken { get; set; }
    }

    public class PaymentResponse
    {
        public long OrderId { get; set; }
        public string CurrencyType { get; set; }
        public string ProviderId { get; set; }
        public string OrderName { get; set; }
        public decimal? PaymentAmount { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}