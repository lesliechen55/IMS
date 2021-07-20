using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace YQTrack.Core.Backend.Admin.WebCore
{
    public class ApiJsonResult : JsonResult
    {
        public ApiJsonResult(ApiResult value) : base(value)
        {
        }

        public ApiJsonResult(ApiResult value, JsonSerializerSettings serializerSettings) : base(value, serializerSettings)
        {
        }
    }
}