namespace YQTrack.Core.Backend.Admin.Message.Service.RemoteApi
{
    public class MessageRequestBase<T>
    {
        public string Version { get; set; }
        public string Method { get; set; }
        public int SourceType { get; set; }
        public T Param { get; set; }
    }
}


