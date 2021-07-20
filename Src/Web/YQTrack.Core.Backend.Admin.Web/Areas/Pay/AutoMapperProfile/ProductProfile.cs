using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
{
    public class ProductProfile : BaseProfile
    {
        public ProductProfile()
        {
            CreateMap<ProductEditRequest, ProductEditInput>();
            CreateMap<ProductEditInput, TProduct>();
            CreateMap<ProductShowOutput, ProductShowResponse>();
            CreateMap<TProduct, ProductShowOutput>().ForMember(
                    dest => dest.FCreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.FUpdateAt,
                    opt => opt.MapFrom(src => src.FUpdateAt.ToLocalTime())
                    );
            CreateMap<ProductPageDataOutput, ProductPageDataResponse>().ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.UpdateAt,
                    opt => opt.MapFrom(src => src.FUpdateAt.ToLocalTime())
                ).ForMember(
                    dest => dest.Role,
                    opt => opt.MapFrom(src => src.FRole.GetDescription())
                ).ForMember(
                    dest => dest.ServiceType,
                    opt => opt.MapFrom(src => src.FServiceType.GetDescription())
                ).ForMember(
                dest => dest.IsSubscription,
                opt => opt.MapFrom(src => src.FIsSubscription ? "是" : "否")
                );
        }
    }
}
