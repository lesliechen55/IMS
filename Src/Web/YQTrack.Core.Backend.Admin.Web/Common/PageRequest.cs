namespace YQTrack.Core.Backend.Admin.Web.Common
{
    public abstract class PageRequest
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}