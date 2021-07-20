namespace YQTrack.Core.Backend.Admin.Deals.DTO
{
    public abstract class PageInput
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}
