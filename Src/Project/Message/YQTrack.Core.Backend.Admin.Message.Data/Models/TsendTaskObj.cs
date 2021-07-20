using YQTrack.Core.Backend.Admin.Message.Core.Enums;

namespace YQTrack.Core.Backend.Admin.Message.Data.Models
{
    public partial class TsendTaskObj
    {
        public long FtaskObjId { get; set; }
        public long FtaskId { get; set; }
        public ObjType FobjType { get; set; }
        public string FobjDetails { get; set; }

        public virtual TsendTask Ftask { get; set; }
    }
}
