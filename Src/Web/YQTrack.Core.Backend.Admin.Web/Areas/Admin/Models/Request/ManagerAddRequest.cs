using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request
{
    public class ManagerAddRequest
    {
        [DisplayName("昵称")]
        public string NickName { get; set; }

        [DisplayName("账号")]
        public string Account { get; set; }

        [DisplayName("密码")]
        public string Password { get; set; }

        [DisplayName("确认密码")]
        public string ConfirmPassword { get; set; }

        [DisplayName("禁用")]
        public bool IsLock { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }

        public string Email { get; set; }
    }
}