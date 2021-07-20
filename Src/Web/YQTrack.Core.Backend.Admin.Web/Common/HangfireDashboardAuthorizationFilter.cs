using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace YQTrack.Core.Backend.Admin.Web.Common
{
    public class HangfireDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// 直接取消hangfire默认的页面授权检查
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Authorize([NotNull] DashboardContext context)
        {
            //var httpcontext = context.GetHttpContext();
            //return httpcontext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}