using System;
using System.Web;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Email.DTO.Input;
using YQTrack.Core.Backend.Admin.Email.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.AutoMapperProfile
{
    public class EmailProfile : BaseProfile
    {
        public EmailProfile()
        {
            CreateMap<EmailPageDataRequest, EmailRecordPageDataInput>();

            CreateMap<EmailRecordPageDataOutput, EmailPageDataResponse>()
                .ForMember(
                    dest => dest.ProviderType,
                    opt => opt.MapFrom(src => src.FProviderType.GetDescription() == "None" ? "无" : src.FProviderType.GetDescription())
                )
                .ForMember(
                    dest => dest.DeliveryFailureStatus,
                    opt => opt.MapFrom(src => src.FDeliveryFailureStatus.HasValue ? src.FDeliveryFailureStatus.Value.GetDescription() == "None" ? "成功" : src.FDeliveryFailureStatus.Value.GetDescription() : string.Empty)
                )
                .ForMember(
                    dest => dest.BusinessEmailFailureStatus,
                    opt => opt.MapFrom(src => src.FBusinessEmailFailureStatus.HasValue ? src.FBusinessEmailFailureStatus.Value.GetDescription() == "None" ? "正常" : src.FBusinessEmailFailureStatus.Value.GetDescription() : string.Empty)
                )
                .ForMember(
                    dest => dest.DeliveryReportedTime,
                    opt => opt.MapFrom(src => src.FDeliveryReportedTime.HasValue ? src.FDeliveryReportedTime.Value.ToLocalTime() : (DateTime?)null)
                )
                .ForMember(
                    dest => dest.BusinessNotifyConfirmTime,
                    opt => opt.MapFrom(src => src.FBusinessNotifyConfirmTime.HasValue ? src.FBusinessNotifyConfirmTime.Value.ToLocalTime() : (DateTime?)null)
                )
                .ForMember(
                    dest => dest.BusinessNotifyConfirmed,
                    opt => opt.MapFrom(src => src.FBusinessNotifyConfirmed ? "是" : "否")
                ).ForMember(
                    dest => dest.BusinessEmailType,
                    opt => opt.MapFrom(src => src.FBusinessEmailType.GetDescription())
                )
                .ForMember(
                    dest => dest.MessageEmailType,
                    opt => opt.MapFrom(src => src.FMessageEmailType.GetDescription())
                )
                .ForMember(
                    dest => dest.SubmitFailureStatus,
                    opt => opt.MapFrom(src => src.FSubmitFailureStatus.GetDescription() == "None" ? "成功" : src.FSubmitFailureStatus.GetDescription())
                ).ForMember(
                    dest => dest.SubmitTime,
                    opt => opt.MapFrom(src => src.FSubmitTime.ToLocalTime())
                ).ForMember(
                    dest => dest.From,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.FFrom))
                ).ForMember(
                    dest => dest.To,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.FTo))
                ).ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.FTitle))
                );

            CreateMap<DeliveryRecordDataOutput, DeliveryRecordDataResponse>()
                .ForMember(
                    dest => dest.ProviderType,
                    opt => opt.MapFrom(src => src.FProviderType.GetDescription() == "None" ? "无" : src.FProviderType.GetDescription())
                ).ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => src.FCreateTime.ToLocalTime())
                );
        }
    }
}