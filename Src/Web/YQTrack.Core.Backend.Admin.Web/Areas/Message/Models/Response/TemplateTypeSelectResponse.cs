using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response
{
    public class TemplateTypeSelectResponse
    {
        public List<ProjectResponse> Projects { get; set; }

        public List<ChannelOutput> Channels { get; set; }
    }
}
