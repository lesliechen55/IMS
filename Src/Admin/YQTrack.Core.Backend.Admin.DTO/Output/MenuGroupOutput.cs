namespace YQTrack.Core.Backend.Admin.DTO.Output
{
    public class MenuOutput
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Href { get; set; }
        public bool Spread { get; set; }
        public MenuOutput[] Children { get; set; }
    }
}