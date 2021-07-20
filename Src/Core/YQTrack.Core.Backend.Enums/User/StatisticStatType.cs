using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.User
{
    /// <summary>
    /// 统计类型
    /// </summary>
    public enum StatisticStatType
    {
        /// <summary>
        /// 日注册用户
        /// </summary>
        [Description("日注册用户")]
        StaUserRegisterByDay = 1,

        /// <summary>
        /// 日活跃用户
        /// </summary>
        [Description("日活跃用户")]
        StaUserPositiveByDay = 2,

        /// <summary>
        /// 日平台账号
        /// </summary>
        [Description("日平台账号")]
        StaPlantAccountByDay = 3,

        /// <summary>
        /// 日订单量
        /// </summary>
        [Description("日订单量")]
        StaOrderByDay = 4,

        /// <summary>
        /// APP日注册用户
        /// </summary>
        [Description("APP日注册用户")]
        StaAppUserRegisterByDay = 5,

        /// <summary>
        /// APP日注册用户
        /// </summary>
        [Description("APP日注册用户")]
        StaAppUserByWeek = 6,

        /// <summary>
        /// 日活跃用户,根据操作日志
        /// </summary>
        [Description("日活跃用户,根据操作日志")]
        StaUserPositiveDayByOperationLog = 7,

        /// <summary>
        /// APP日活跃用户,根据设备类型分类
        /// </summary>
        [Description("APP日活跃用户,根据设备类型分类")]
        StaAppUserPositiveByDay = 8
    }
}