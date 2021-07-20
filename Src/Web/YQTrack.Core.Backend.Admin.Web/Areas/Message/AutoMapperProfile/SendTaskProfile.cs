using System;
using YQTrack.Backend.Message.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.Core.Enums;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.AutoMapperProfile
{
    public class SendTaskProfile : BaseProfile
    {
        public SendTaskProfile()
        {
            CreateMap<SendTaskEditRequest, SendTaskEditInput>();

            CreateMap<SendTemplateTestRequest, SendTemplateTestInput>();

            CreateMap<SendTaskEditOutput, SendTaskEditResponse>();

            CreateMap<SendTaskPageDataInput, SendTaskPageDataRequest>();

            CreateMap<SendTaskPageDataOutput, SendTaskPageDataResponse>()
                .ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => src.FCreateTime.HasValue ? src.FCreateTime.Value.ToLocalTime() : (DateTime?)null)
                ).ForMember(
                    dest => dest.UpdateTime,
                    opt => opt.MapFrom(src => src.FUpdateTime.HasValue ? src.FUpdateTime.Value.ToLocalTime() : (DateTime?)null)
                ).ForMember(
                    dest => dest.Channel,
                    opt => opt.MapFrom(src => ((ChannelSend)src.FChannel).GetDescription())
                ).ForMember(
                    dest => dest.StateName,
                    opt => opt.MapFrom(src => ((PushState)src.FState).GetDescription())
                );
        }
    }
}