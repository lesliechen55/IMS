using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
{
    public class PurchaseOrderProfile : BaseProfile
    {
        public PurchaseOrderProfile()
        {
            CreateMap<PurchaseOrderPageDataRequest, PurchaseOrderPageDataInput>();
            CreateMap<PurchaseOrderPageDataOutput, PurchaseOrderPageDataResponse>()
                .ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.UpdateAt,
                    opt => opt.MapFrom(src => src.FUpdateAt.ToLocalTime())
                    )
                .ForMember(
                    dest => dest.UserPlatformType,
                    opt => opt.MapFrom(src => src.FUserPlatformType.GetDescription())
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
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.FStatus.GetDisplayName())
                )
                .ForMember(
                    dest => dest.ProviderId,
                    opt => opt.MapFrom(src => src.FProviderId.GetDescription())
                ).ForMember(
                    dest => dest.Conflict,
                    opt => opt.MapFrom(src => src.FIsSubscriptionConflict ? "是" : "否")
                );
            CreateMap<TPurchaseOrder, PurchaseOrderShowOutput>()
                .ForMember(
                    dest => dest.TPurchaseOrderItem,
                    opt => opt.MapFrom(src => src.TPurchaseOrderItem)
                );

            CreateMap<PurchaseOrderShowOutput, PurchaseOrderShowResponse>()
                .ForMember(
                    dest => dest.UserPlatformType,
                    opt => opt.MapFrom(src => src.FUserPlatformType.GetDescription())
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
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.FStatus.GetDisplayName())
                )
                .ForMember(
                    dest => dest.PurchaseOrderItem,
                    opt => { opt.MapFrom(src => src.TPurchaseOrderItem); }
                ).ForMember(
                    dest => dest.IsSubscriptionConflict,
                    opt => { opt.MapFrom(src => src.FIsSubscriptionConflict ? "是" : "否"); }
                );

            CreateMap<PurchaseOrderItemOutput, PurchaseOrderItemResponse>()
                .ForMember(
                    dest => dest.CurrencyType,
                    opt => opt.MapFrom(src => src.FCurrencyType.GetDescription())
                )
                .ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                    )
                .ForMember(
                    dest => dest.StartTime,
                    opt => opt.MapFrom(src => src.FStartTime.HasValue ? src.FStartTime.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : string.Empty)
                ).ForMember(
                    dest => dest.StopTime,
                    opt => opt.MapFrom(src => src.FStopTime.HasValue ? src.FStopTime.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : string.Empty)
                );
        }
    }
}
