using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace YQTrack.Core.Backend.Enums.User
{
    /// <summary>
    /// 用户平台类型
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    [Flags]
    public enum UserPlatformType
    {
        /// <summary>未知</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UNKNOWN")]
        [Display(Name = "UnKnown")]
        [Description("未知")]
        UNKNOWN = 0,

        /// <summary>Web</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "WEB")]
        [Display(Name = "Web")]
        [Description("Web")]
        WEB = 0x1,

        /// <summary>Android</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ANDROID")]
        [Display(Name = "Android")]
        [Description("Android")]
        ANDROID = 0x2,

        /// <summary>IOS</summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IOS")]
        [Display(Name = "iOS")]
        [Description("iOS")]
        IOS = 0x4,
    }
}
