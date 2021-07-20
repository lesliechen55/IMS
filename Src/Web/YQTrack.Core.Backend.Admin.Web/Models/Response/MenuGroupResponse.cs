namespace YQTrack.Core.Backend.Admin.Web.Models.Response
{
    public class MenuResponse
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Href { get; set; }
        public bool Spread { get; set; }
        public MenuResponse[] Children { get; set; }
    }
}