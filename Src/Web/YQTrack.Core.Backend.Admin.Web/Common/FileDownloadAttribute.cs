using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace YQTrack.Core.Backend.Admin.Web.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class FileDownloadAttribute : ActionFilterAttribute
    {
        private readonly string _cookieName;

        private readonly string _cookiePath;

        public FileDownloadAttribute(string cookieName = "fileDownload", string cookiePath = "/")
        {
            _cookieName = cookieName;
            _cookiePath = cookiePath;
        }

        /// <summary>
        /// If the current response is a FileResult (an MVC base class for files) then write a
        /// cookie to inform jquery.fileDownload that a successful file download has occured
        /// </summary>
        /// <param name="filterContext"></param>
        private void CheckAndHandleFileResult(ActionExecutedContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            var response = httpContext.Response;

            if (filterContext.Result is FileResult)
            {
                //jquery.fileDownload uses this cookie to determine that a file download has completed successfully
                response.Cookies.Append(_cookieName, "true", new CookieOptions() { Path = _cookiePath });
            }
            else
            {
                //ensure that the cookie is removed in case someone did a file download without using jquery.fileDownload
                if (httpContext.Request.Cookies[_cookieName] != null)
                {
                    response.Cookies.Delete(_cookieName, new CookieOptions() { Expires = DateTime.Now.AddYears(-1), Path = _cookiePath });
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            CheckAndHandleFileResult(filterContext);

            base.OnActionExecuted(filterContext);
        }
    }
}