using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace YQTrack.Core.Backend.Admin.Web.Common
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _realm;

        public BasicAuthMiddleware(RequestDelegate next, string realm)
        {
            this._next = next;
            this._realm = realm;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.HasValue)
            {
                var pathValue = context.Request.Path.Value;
                if (pathValue.StartsWith("/log") || pathValue.StartsWith("/swagger") || pathValue.StartsWith("/hangfire") || pathValue.StartsWith("/reconcile"))
                {
                    string authHeader = context.Request.Headers["Authorization"];
                    if (authHeader != null && authHeader.StartsWith("Basic "))
                    {
                        // Get the encoded username and password
                        var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                        // Decode from Base64 to string
                        var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                        // Split username and password
                        var username = decodedUsernamePassword.Split(':', 2)[0];
                        var password = decodedUsernamePassword.Split(':', 2)[1];

                        // Check if login is correct
                        if (IsAuthorized(username, password))
                        {
                            await _next.Invoke(context);
                            return;
                        }
                    }

                    // Return authentication type (causes browser to show login dialog)
                    context.Response.Headers["WWW-Authenticate"] = "Basic";

                    // Add realm if it is not null
                    if (!string.IsNullOrWhiteSpace(_realm))
                    {
                        context.Response.Headers["WWW-Authenticate"] += $" realm=\"{_realm}\"";
                    }

                    // Return unauthorized
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                    return;
                }
            }

            await _next.Invoke(context);
        }

        private static bool IsAuthorized(string username, string password)
        {
            return username == "admin" && password == "17track";
        }
    }
}