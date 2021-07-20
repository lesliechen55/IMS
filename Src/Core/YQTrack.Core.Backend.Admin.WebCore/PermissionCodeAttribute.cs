using System;
using System.Linq;

namespace YQTrack.Core.Backend.Admin.WebCore
{
    /// <summary>
    /// 权限代码标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class PermissionCodeAttribute : Attribute
    {
        private readonly string[] _permissionNames;

        public PermissionCodeAttribute(params string[] permissionNames)
        {
            if (permissionNames == null || !permissionNames.Any())
            {
                throw new ArgumentNullException(nameof(permissionNames));
            }
            _permissionNames = permissionNames;
        }

        public string[] PermissionNames => _permissionNames;
    }
}