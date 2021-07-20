using System;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.Core.Enums;
using YQTrack.Core.Backend.Admin.Message.Data.Models;
using YQTrack.Core.Backend.Admin.Message.DTO.Input;
using YQTrack.Core.Backend.Admin.Message.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Message.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Message.AutoMapperProfile
{
    public class TemplateTypeProfile : BaseProfile
    {
        public TemplateTypeProfile()
        {
            CreateMap<ProjectOutput, ProjectResponse>();

            CreateMap<TtemplateType, TemplateTypeEditOutput>();

            CreateMap<TemplateTypeEditInput, TtemplateType>();

            CreateMap<TemplateTypeEditOutput, TemplateTypeEditResponse>();

            CreateMap<TemplateTypeEditRequest, TemplateTypeEditInput>();

            CreateMap<TemplateTypePageDataOutput, TemplateTypePageDataResponse>()
                .ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => src.FCreateTime.HasValue ? src.FCreateTime.Value.ToLocalTime() : (DateTime?)null)
                ).ForMember(
                    dest => dest.UpdateTime,
                    opt => opt.MapFrom(src => src.FUpdateTime.HasValue ? src.FUpdateTime.Value.ToLocalTime() : (DateTime?)null)
                ).ForMember(
                    dest => dest.TemplateNo,
                    opt => opt.MapFrom(src => $"{src.FProjectName} {src.FTemplateTypeId}")
                ).ForMember(
                    dest => dest.ChannelName,
                    opt => opt.MapFrom(src => src.FChannel.GetDescription())
                ).ForMember(
                    dest => dest.Enable,
                    opt => opt.MapFrom(src => ((TemplateState)src.FEnable).GetDescription())
                );
        }
    }
}