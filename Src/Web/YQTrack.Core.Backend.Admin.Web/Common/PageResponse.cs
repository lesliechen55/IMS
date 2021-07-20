using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.WebCore;

namespace YQTrack.Core.Backend.Admin.Web.Common
{
    public class PageResponse<T> : ApiResult where T : class, new()
    {
        public PageResponse()
        {
            Code = 0;
            Msg = string.Empty;
        }
        public PageResponse(IEnumerable<T> data, int count) : base()
        {
            Data = data;
            Count = count;
        }
        public int Code { get; set; }
        public int Count { get; set; }
        public IEnumerable<T> Data { get; set; } = new List<T>();
    }
}