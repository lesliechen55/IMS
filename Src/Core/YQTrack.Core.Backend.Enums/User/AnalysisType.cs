using System.ComponentModel;

namespace YQTrack.Core.Backend.Enums.User
{
    public enum AnalysisType
    {
        /// <summary>
        /// WEB日注册用户
        /// </summary>
        [Description("WEB日注册用户")]
        WebUserRegisterByDay = 1,

        /// <summary>
        /// 日活跃用户
        /// </summary>
        [Description("日活跃用户")]
        UserPositiveByDay = 2,

        /// <summary>
        /// 日平台账号
        /// </summary>
        [Description("日平台账号")]
        PlatformAccountByDay = 3,

        /// <summary>
        /// APP日注册用户
        /// </summary>
        [Description("APP日注册用户")]
        AppUserRegisterByDay = 5,

        /// <summary>
        /// APP日活跃用户
        /// </summary>
        [Description("APP日活跃用户")]
        AppUserPositiveByDay = 8
    }
}