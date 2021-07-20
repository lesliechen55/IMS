using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Models.Request
{
    public class ChangePwdRequest
    {
        [DisplayName("旧密码")]
        public string OldPassword { get; set; }

        [DisplayName("新密码")]
        public string NewPassword { get; set; }

        [DisplayName("确认密码")]
        public string ConfirmPassword { get; set; }
    }
}