using YQTrack.Core.Backend.Admin.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.AutoMapperProfile
{
    public class ESFieldProfile : BaseProfile
    {
        public ESFieldProfile()
        {

            CreateMap<ESFieldOutput, ESFieldResponse>();
        }
    }
}