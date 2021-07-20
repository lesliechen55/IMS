using System.Text.RegularExpressions;

namespace YQTrack.Core.Backend.Admin.Web.Common
{
    public static class AppConfig
    {
        /// <summary>
        /// 密码正则验证器
        /// </summary>
        public static readonly Regex PasswordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,16}$", RegexOptions.Compiled);

        /// <summary>
        /// 发行者签名密钥
        /// </summary>
        public const string IssuerSigningKey = "JwtBearerSample_11231~#$%#%^2235";

        /// <summary>
        /// 发行者
        /// </summary>
        public const string Issuer = "17track";

        /// <summary>
        /// 受众
        /// </summary>
        public const string Audience = "Api";
    }
}