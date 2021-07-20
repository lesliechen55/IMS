using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
{
    public class PaymentProfile : BaseProfile
    {
        public PaymentProfile()
        {
            CreateMap<PaymentPageDataRequest, PaymentPageDataInput>();
            CreateMap<TPayment, PaymentShowOutput>();
            CreateMap<PaymentPageDataOutput, PaymentPageDataResponse>().ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.UpdateAt,
                    opt => opt.MapFrom(src => src.FUpdateAt.ToLocalTime())
                    )
                .ForMember(
                    dest => dest.PlatformType,
                    opt => opt.MapFrom(src => src.FPlatformType.GetDescription())
                )
                .ForMember(
                    dest => dest.CurrencyType,
                    opt => opt.MapFrom(src => src.FCurrencyType.GetDescription())
                )
                .ForMember(
                    dest => dest.ProviderId,
                    opt => opt.MapFrom(src => src.FProviderId.GetDescription())
                )
                .ForMember(
                    dest => dest.ServiceType,
                    opt => opt.MapFrom(src => src.FServiceType.GetDescription())
                )
                .ForMember(
                    dest => dest.PaymentStatus,
                    opt => opt.MapFrom(src => src.FPaymentStatus.GetDisplayName())
                );

            CreateMap<PaymentShowOutput, PaymentShowResponse>()
                .ForMember(
                    dest => dest.PlatformType,
                    opt => opt.MapFrom(src => src.FPlatformType.GetDescription())
                )
                .ForMember(
                    dest => dest.CurrencyType,
                    opt => opt.MapFrom(src => src.FCurrencyType.GetDescription())
                )
                .ForMember(
                    dest => dest.ServiceType,
                    opt => opt.MapFrom(src => src.FServiceType.GetDescription())
                )
                .ForMember(
                    dest => dest.PaymentStatus,
                    opt => opt.MapFrom(src => src.FPaymentStatus.GetDisplayName())
                )
                .ForMember(
                    dest => dest.ProviderId,
                    opt => { opt.MapFrom(src => src.FProviderId.GetDescription()); }
                )
                .ForMember(
                    dest => dest.PaymentLog,
                    opt => opt.MapFrom(src => src.PaymentLog)
                )
                .ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                )
                .ForMember(
                    dest => dest.UpdateAt,
                    opt => opt.MapFrom(src => src.FUpdateAt.ToLocalTime())
                );

            CreateMap<PaymentLogOutput, PaymentLogResponse>()
                .ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                );
        }
    }
}
