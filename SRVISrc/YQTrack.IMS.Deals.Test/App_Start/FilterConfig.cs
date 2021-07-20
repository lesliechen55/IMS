using System.Web.Mvc;

namespace YQTrack.IMS.Deals.Test
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new GlobalPermissionActionFilter("17track", "A7Y6KXJEmF6JNuy@", "http://localhost:5000/Remote/CheckPermission"));
        }
    }
}
