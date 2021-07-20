using System.ComponentModel;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request
{
    public class ManagerSetRoleRequest
    {
        [DisplayName("管理员ID")]
        public int Id { get; set; }

        [DisplayName("角色ID数据集")]
        public int[] RoleIdList { get; set; }
    }
}