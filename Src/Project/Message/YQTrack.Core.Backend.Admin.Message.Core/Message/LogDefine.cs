using System.Diagnostics.CodeAnalysis;
using YQTrack.Log;

namespace YQTrack.Core.Backend.Admin.Message.Core.Message
{
    /// <summary>
    /// 日志信息定义
    /// </summary>
    internal static class LogDefine
    {
        /// <summary>
        /// Error
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static LogDefinition LogError = new LogDefinition(LogLevel.Error, "消息发送帮助库,异常:{0}");

        /// <summary>
        /// Warn
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static LogDefinition LogWarn = new LogDefinition(LogLevel.Warn, "消息发送帮助库,警告:{0}");

        /// <summary>
        /// Verbose
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static LogDefinition LogVerbose = new LogDefinition(LogLevel.Verbose, "消息发送帮助库,详细:{0}");
    }
}
