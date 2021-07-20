using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
{
    public class OfflinePaymentProfile : BaseProfile
    {
        public OfflinePaymentProfile()
        {
            CreateMap<OfflinePaymentPageDataRequest, OfflinePaymentPageDataInput>();
            CreateMap<OfflinePaymentPassRequest, OfflinePaymentPassInput>();
            CreateMap<OfflinePaymentRejectRequest, OfflinePaymentRejectInput>();
            CreateMap<OfflinePaymentShowOutput, OfflinePaymentShowResponse>()
                .ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.CurrencyType,
                    opt => opt.MapFrom(src => src.FCurrencyType.GetDisplayName())
                ).ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.FStatus.GetDescription() + (src.FStatus == OfflineAuditStatus.Rejected ? $"({src.FRejectReason})" : string.Empty))
                );
            CreateMap<OfflinePaymentOrderOutput, OfflinePaymentOrderResponse>()
                .ForMember(
                    dest => dest.CurrencyType,
                    opt => opt.MapFrom(src => src.FCurrencyType.GetDisplayName())
                )
                .ForMember(
                    dest => dest.EffectiveTime,
                    opt => opt.MapFrom(src => src.FEffectiveTime.HasValue ? src.FEffectiveTime.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : "--")
                );
            CreateMap<OfflinePaymentPageDataOutput, OfflinePaymentPageDataResponse>()
                .ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.CurrencyType,
                    opt => opt.MapFrom(src => src.FCurrencyType.GetDisplayName())
                ).ForMember(
                    dest => dest.ProviderId,
                    opt => opt.MapFrom(src => src.FProviderId.GetDescription())
                ).ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.FStatus.GetDescription() + (src.FStatus == OfflineAuditStatus.Rejected ? $"({src.FRejectReason})" : string.Empty))
                ).ForMember(
                    dest => dest.HandleTime,
                    opt => opt.MapFrom(src => src.FHandleTime.HasValue ? src.FHandleTime.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : "--")
                );
        }
    }
}
