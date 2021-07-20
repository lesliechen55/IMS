namespace YQTrack.Core.Backend.Admin.CarrierTrack.JobService.Dto
{
    public class ImportDbQueryDto
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string DateFormat { get; set; }
        public int SuccessInsertTotal { get; set; }
    }
}