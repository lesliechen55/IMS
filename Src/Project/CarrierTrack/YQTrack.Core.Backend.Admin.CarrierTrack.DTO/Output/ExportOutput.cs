using System;
using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Output
{
    public class ReportOutput
    {
        public DateTime Date { get; set; }
        public List<UserImportOutput> UserImportOutputs { get; set; }
    }

    public class UserImportOutput
    {
        public string Email { get; set; }
        public int Import { get; set; }
    }
}