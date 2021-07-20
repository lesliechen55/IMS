using System;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Message.Data.Models
{
    public partial class TtemplateType
    {
        public TtemplateType()
        {
            Ttemplate = new HashSet<Ttemplate>();
        }

        public long FtemplateTypeId { get; set; }
        public long FprojectId { get; set; }
        public int Fchannel { get; set; }
        public string FtemplateName { get; set; }
        public string FtemplateDescribe { get; set; }
        public int Fenable { get; set; }
        public int? FisRendering { get; set; }
        public DateTime? FcreateTime { get; set; }
        public DateTime? FupdateTime { get; set; }
        public string FdataJson { get; set; }
        public int? FtemplateCode { get; set; }
        public string FtemplateTitle { get; set; }
        public string FtemplateBody { get; set; }

        public virtual Tproject Fproject { get; set; }
        public virtual ICollection<Ttemplate> Ttemplate { get; set; }
    }
}
