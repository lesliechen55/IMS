using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Models.Api.Request
{
    public class TokenRequest
    {
        /// <summary>
        /// 账号
        /// </summary>
        [DisplayName("账号")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [DisplayName("密码")]
        public string Password { get; set; }
    }
}