namespace YQTrack.Core.Backend.Admin.Web.Common
{
    public class IframeTransferData
    {
        public string InvokeElementId { get; set; }

        public string Id { get; set; }
    }

    public class IframeTransferData<T> : IframeTransferData where T : class,new()
    {
        public T Data { get; set; }
    }
}