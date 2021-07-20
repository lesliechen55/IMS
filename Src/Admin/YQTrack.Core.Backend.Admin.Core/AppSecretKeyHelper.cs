using System;
using YQTrack.Utility;

namespace YQTrack.Core.Backend.Admin.Core
{
    public class AppSecretKeyHelper
    {
        /// <summary>
        /// 生成用户访问密钥
        /// </summary>
        /// <param name="userNo">用户编号</param>
        /// <param name="secretSeed">密钥种子</param>
        /// <returns></returns>
        public static string GetSecretKey(int userNo, DateTime? secretSeed)
        {
            if (secretSeed.HasValue)
            {
                return SecurityExtend.MD5Encrypt($"{ userNo.ToString()}{secretSeed.Value.ToString("yyyy-MM-ddTHH:mm:ss.fff")}");
            }
            else
            {
                return SecurityExtend.MD5Encrypt($"{ userNo.ToString()}");
            }
        }
    }
}
