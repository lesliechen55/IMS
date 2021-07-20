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
    public class InvoiceApplyProfile : BaseProfile
    {
        public InvoiceApplyProfile()
        {
            CreateMap<InvoiceApplyPageDataRequest, InvoiceApplyPageDataInput>();
            CreateMap<InvoiceApplyPassRequest, InvoiceApplyPassInput>();
            CreateMap<InvoiceApplyRejectRequest, InvoiceApplyRejectInput>();
            CreateMap<InvoiceApplyShowOutput, InvoiceApplyShowResponse>()
                .ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.InvoiceType,
                    opt => opt.MapFrom(src => src.FInvoiceType.GetDescription())
                ).ForMember(
                    dest => dest.CurrencyType,
                    opt => opt.MapFrom(src => src.FCurrencyType.GetDisplayName())
                ).ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.FStatus.GetDescription() + (src.FStatus == InvoiceAuditStatus.Rejected ? $"({src.FRejectReason})" : string.Empty))
                ).ForMember(
                    dest => dest.SendInfo,
                    opt => opt.MapFrom(src => src.FStatus == InvoiceAuditStatus.Approved ? $"{src.FSendType.GetDescription()} => {src.FSendInfo}" : string.Empty)
                );
            CreateMap<InvoicePaymentOutput, InvoicePaymentResponse>()
                .ForMember(
                    dest => dest.CurrencyType,
                    opt => opt.MapFrom(src => src.FCurrencyType.GetDisplayName())
                )
                .ForMember(
                    dest => dest.PaymentCreateTime,
                    opt => opt.MapFrom(src => src.FPaymentCreateTime.ToLocalTime())
                ).ForMember(
                    dest => dest.Provider,
                    opt => opt.MapFrom(src => src.PaymentProvider.GetDescription())
                );
            CreateMap<InvoiceApplyPageDataOutput, InvoiceApplyPageDataResponse>()
                .ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.InvoiceType,
                    opt => opt.MapFrom(src => src.FInvoiceType.GetDescription())
                ).ForMember(
                    dest => dest.CurrencyType,
                    opt => opt.MapFrom(src => src.FCurrencyType.GetDisplayName())
                ).ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.FStatus.GetDescription() + (src.FStatus == InvoiceAuditStatus.Rejected ? $"({src.FRejectReason})" : string.Empty))
                ).ForMember(
                    dest => dest.SendInfo,
                    opt => opt.MapFrom(src => src.FStatus == InvoiceAuditStatus.Approved ? string.Format(src.FSendType.GetDescription(), src.FSendInfo) : "--")
                ).ForMember(
                    dest => dest.HandleTime,
                    opt => opt.MapFrom(src => src.FHandleTime.HasValue ? src.FHandleTime.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : "--")
                );
        }
    }
}
