using System;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Message.Data.Models
{
    public partial class TsendTask
    {
        public TsendTask()
        {
            TsendTaskObj = new HashSet<TsendTaskObj>();
        }

        public long FtaskId { get; set; }
        public long FtemplateTypeId { get; set; }
        public DateTime? FcreateTime { get; set; }
        public DateTime? FpushTime { get; set; }
        public DateTime? FupdateTime { get; set; }
        public int? FpushSucess { get; set; }
        public int? FpushFail { get; set; }
        public string Fremarks { get; set; }
        public int? Fstate { get; set; }
        public int? FretryCount { get; set; }
        public string FdataJson { get; set; }
        public string FCreateBy { get; set; }
        public string FUpdateBy { get; set; }

        public virtual ICollection<TsendTaskObj> TsendTaskObj { get; set; }
    }
}
