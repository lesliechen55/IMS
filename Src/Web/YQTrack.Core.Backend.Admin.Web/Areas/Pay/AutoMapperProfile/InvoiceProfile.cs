using System;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
{
    public class InvoiceProfile : BaseProfile
    {
        public InvoiceProfile()
        {
            CreateMap<InvoicePageDataRequest, InvoicePageDataInput>();
            CreateMap<InvoiceEditRequest, InvoiceEditInput>();
            CreateMap<InvoiceAddRequest, InvoiceAddInput>();
            CreateMap<InvoiceShowOutput, InvoiceShowResponse>();
            CreateMap<InvoicePageDataOutput, InvoicePageDataResponse>()
                .ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.UpdateAt,
                    opt => opt.MapFrom(src => src.FUpdateAt.HasValue ? src.FUpdateAt.Value.ToLocalTime() : (DateTime?)null)
                ).ForMember(
                    dest => dest.InvoiceType,
                    opt => opt.MapFrom(src => src.FInvoiceType.GetDescription())
                );
            CreateMap<TInvoice, InvoiceShowOutput>();
            CreateMap<InvoiceAddInput, TInvoice>();
            CreateMap<InvoiceEditInput, TInvoice>();
        }
    }
}
