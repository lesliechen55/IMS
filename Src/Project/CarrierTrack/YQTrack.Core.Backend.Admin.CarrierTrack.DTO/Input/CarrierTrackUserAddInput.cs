namespace YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Input
{
    public class CarrierTrackUserAddInput
    {
        public string FEmail { get; set; }
        public int FImportTodayLimit { get; set; }
        public int FExportTimeLimit { get; set; }
        public bool FEnable { get; set; }
    }
}