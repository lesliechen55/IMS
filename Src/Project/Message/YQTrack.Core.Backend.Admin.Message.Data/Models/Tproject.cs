using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Message.Data.Models
{
    public partial class Tproject
    {
        public Tproject()
        {
            TtemplateType = new HashSet<TtemplateType>();
        }

        public long FprojectId { get; set; }
        public long FparentId { get; set; }
        public string FprojectName { get; set; }
        public string Fprefix { get; set; }

        public virtual ICollection<TtemplateType> TtemplateType { get; set; }
    }
}
