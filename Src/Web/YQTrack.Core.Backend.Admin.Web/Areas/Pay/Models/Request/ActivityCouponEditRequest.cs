using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request
{
    public class ActivityCouponEditRequest
    {
        public long? ActivityId { get; set; }
        public string Email { get; set; }

        public ICollection<Rule> Rules { get; set; }
        public ActivityCouponEditRequest()
        {
            Rules = new List<Rule>();
        }
    }
}