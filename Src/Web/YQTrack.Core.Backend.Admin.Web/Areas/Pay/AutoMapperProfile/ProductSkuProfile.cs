using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
{
    public class ProductSkuProfile : BaseProfile
    {
        public ProductSkuProfile()
        {
            CreateMap<ProductSkuPageDataRequest, ProductSkuPageDataInput>();

            CreateMap<ProductSkuPageDataOutput, ProductSkuPageDataResponse>()
                .ForMember(
                    dest => dest.UpdateAt,
                    opt => opt.MapFrom(src => src.FUpdateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.MemberLevel,
                    opt => opt.MapFrom(src => src.FMemberLevel.GetDescription())
                ).ForMember(
                    dest => dest.SkuType,
                    opt => opt.MapFrom(src => src.SkuType.GetDescription())
                ).ForMember(
                    dest => dest.IsInternalUse,
                    opt => opt.MapFrom(src => src.IsInternalUse ? "是" : "否")
                );

            CreateMap<ProductSkuAddRequest, ProductSkuAddInput>();
            CreateMap<ProductSkuAddInput, TProductSku>()
                .ForMember(
                    dest => dest.FDescription,
                    opt => opt.MapFrom(src => src.Desc)
                ).ForMember(
                    dest => dest.FType,
                    opt => opt.MapFrom(src => src.SkuType)
                );

            CreateMap<TProductSku, ProductSkuEditOutput>();
            CreateMap<ProductSkuEditOutput, ProductSkuEditResponse>();

            CreateMap<TProductSkuPrice, ProductSkuPriceOutput>();
            CreateMap<ProductSkuPriceOutput, ProductSkuPriceResponse>()
                .ForMember(
                    dest => dest.PlatformType,
                    opt => opt.MapFrom(src => src.FPlatformType.GetDisplayName())
                ).ForMember(
                    dest => dest.CurrencyType,
                    opt => opt.MapFrom(src => src.FCurrencyType.GetDisplayName())
                ).ForMember(
                    dest => dest.UpdateAt,
                    opt => opt.MapFrom(src => src.FUpdateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                );

            CreateMap<ProductSkuEditRequest, ProductSkuEditInput>();
            CreateMap<ProductSkuEditInput, TProductSku>()
                .ForMember(
                    dest => dest.FDescription,
                    opt => opt.MapFrom(src => src.Desc)
                )
                .ForMember(
                    dest => dest.FType,
                    opt => opt.MapFrom(src => src.SkuType)
                );

            CreateMap<ProductSkuAddPriceRequest, ProductSkuAddPriceInput>()
                .ForMember(
                    dest => dest.FSaleUnitPrice,
                    opt => opt.MapFrom(src => decimal.Round(src.SaleUnitPrice, 2))
                ).ForMember(
                    dest => dest.FAmount,
                    opt => opt.MapFrom(src => decimal.Round(src.Amount, 2))
                );
            CreateMap<ProductSkuAddPriceInput, TProductSkuPrice>();

            CreateMap<ProductSkuAddBusinessRequest, ProductSkuAddBusinessInput>();
            CreateMap<ProductSkuAddBusinessInput, ProductSkuBusinessOutput>();

        }
    }
}