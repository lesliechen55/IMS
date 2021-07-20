using System;
using System.Collections.Generic;
using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Core.Backend.Admin.Message.Core.Enums;

namespace YQTrack.Core.Backend.Admin.Message.Data.Models
{
    public partial class TsiteMessageDetails
    {
        public long FsiteMessageDetailsId { get; set; }
        public long FsiteMessageId { get; set; }
        public long FtemplateTypeId { get; set; }
        public DateTime FcreateTime { get; set; }
        public DateTime? Foverdue { get; set; }
        public long FuserId { get; set; }
        public string FdataJson { get; set; }
        public SiteMessageState FisRead { get; set; }
        public Del FisDel { get; set; }
        public DateTime? FupdateTime { get; set; }
    }
}
