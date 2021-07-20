using Microsoft.AspNetCore.Http;

namespace YQTrack.Core.Backend.Admin.Core
{
    public static class HttpContextExtension
    {
        public static string GetReadIpAddress(this IHttpContextAccessor httpContextAccessor)
        {
            string result = httpContextAccessor.HttpContext?.Request?.Headers["CF-Connecting-IP"];
            if (!string.IsNullOrEmpty(result))
                return result;

            result = httpContextAccessor.HttpContext?.Request?.Headers["X-Real-IP"];
            if (!string.IsNullOrEmpty(result))
                return result;

            result = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();
            return result;
        }
    }
}