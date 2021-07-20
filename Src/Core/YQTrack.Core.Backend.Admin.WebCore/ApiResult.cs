namespace YQTrack.Core.Backend.Admin.WebCore
{
    public class ApiResult
    {
        public bool Success { get; set; } = true;
        public string Msg { get; set; } = string.Empty;
    }

    public class ApiResult<TData> : ApiResult
    {
        public ApiResult() { }
        public TData Data { get; set; }
        public ApiResult(TData data)
        {
            Data = data;
        }
    }
}