using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request
{
    public class ManagerEditRequest
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("昵称")]
        public string NickName { get; set; }

        [DisplayName("密码")]
        public string Password { get; set; }

        [DisplayName("确认密码")]
        public string ConfirmPassword { get; set; }

        public bool IsLock { get; set; }

        [DisplayName("备注")]
        public string Remark { get; set; }
    }
}