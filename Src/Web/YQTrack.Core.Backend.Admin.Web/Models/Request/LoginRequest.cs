using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Models.Request
{
    public class LoginRequest
    {
        [DisplayName("用户名")]
        public string Account { get; set; }

        [DisplayName("密码")]
        public string Password { get; set; }

        [DisplayName("验证码")]
        public string Code { get; set; }

        [DisplayName("记住我")] public bool RememberMe { get; set; } = false;
    }
}