using System.ComponentModel;
using YQTrack.Configuration;
using YQTrack.Storage.QiniuOSS;

namespace YQTrack.Core.Backend.Admin.Core
{
    [Category("UserQiniuConfig")]
    public class UserQiniuConfig : QiniuConfig { }

    public static class QiniuHelper
    {
        public static QiniuStorage QiniuStorage = new QiniuStorage(ConfigManager.Initialize<UserQiniuConfig>());
    }
}
