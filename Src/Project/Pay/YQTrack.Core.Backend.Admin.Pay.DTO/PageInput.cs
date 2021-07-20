namespace YQTrack.Core.Backend.Admin.Pay.DTO
{
    public abstract class PageInput
    {
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}
