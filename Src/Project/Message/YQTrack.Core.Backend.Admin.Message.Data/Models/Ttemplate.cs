namespace YQTrack.Core.Backend.Admin.Message.Data.Models
{
    public partial class Ttemplate
    {
        public long FtemplateId { get; set; }
        public long FtemplateTypeId { get; set; }
        public string Flanguage { get; set; }
        public string FtemplateTitle { get; set; }
        public string FtemplateBody { get; set; }
        public int? FisDel { get; set; }
        public string FTemplateData { get; set; }

        public virtual TtemplateType FtemplateType { get; set; }
    }
}
