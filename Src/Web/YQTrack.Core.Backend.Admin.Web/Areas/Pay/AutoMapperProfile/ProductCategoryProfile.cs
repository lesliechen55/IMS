using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
{
    public class ProductCategoryProfile : BaseProfile
    {
        public ProductCategoryProfile()
        {
            CreateMap<ProductCategoryPageDataRequest, ProductCategoryPageDataInput>();

            CreateMap<ProductCategoryPageDataOutput, ProductCategoryPageDataResponse>()
                .ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.UpdateAt,
                    opt => opt.MapFrom(src => src.FUpdateAt.ToLocalTime())
                );

            CreateMap<ProductCategoryAddRequest, ProductCategoryAddInput>();
            CreateMap<ProductCategoryAddInput, TProductCategory>()
                .ForMember(
                    dest => dest.FDescription,
                    opt => opt.MapFrom(src => src.Desc)
                );

            CreateMap<TProductCategory, ProductCategoryOutput>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.FProductCategoryId)
                );
            CreateMap<ProductCategoryOutput, ProductCategoryResponse>();
        }
    }
}