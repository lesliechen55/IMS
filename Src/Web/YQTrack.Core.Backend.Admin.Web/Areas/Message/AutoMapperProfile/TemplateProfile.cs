using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.AutoMapperProfile
{
    public class TemplateProfile : BaseProfile
    {
        public TemplateProfile()
        {
            CreateMap<TemplatePageDataRequest, TemplatePageDataInput>();

            CreateMap<TemplateShowOutput, TemplateShowResponse>();

            CreateMap<TemplatePageDataOutput, TemplatePageDataResponse>()
                .ForMember(
                    dest => dest.ChannelName,
                    opt => opt.MapFrom(src => src.FChannel.GetDescription())
                );
        }
    }
}