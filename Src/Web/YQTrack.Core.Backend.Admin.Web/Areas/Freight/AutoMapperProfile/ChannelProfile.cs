using System;
using System.Web;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.AutoMapperProfile
{
    public class ChannelProfile : BaseProfile
    {
        public ChannelProfile()
        {
            CreateMap<ChannelPageDataRequest, ChannelPageDataInput>();

            CreateMap<ChannelPageDataOutput, ChannelPageDataResponse>()
                .ForMember(
                    dest => dest.PublishTime,
                    opt => opt.MapFrom(src => src.FpublishTime.HasValue ? src.FpublishTime.Value.ToLocalTime() : (DateTime?)null)
                ).ForMember(
                    dest => dest.ExpireTime,
                    opt => opt.MapFrom(src => src.FexpireTime.ToLocalTime())
                ).ForMember(
                    dest => dest.ProductType,
                    opt => opt.MapFrom(src => ((ProductType)src.FproductType).GetDescription())
                ).ForMember(
                    dest => dest.FreightType,
                    opt => opt.MapFrom(src => ((FreightType)src.FfreightType).GetDescription())
                ).ForMember(
                    dest => dest.State,
                    opt => opt.MapFrom(src => ((ChannelState)src.Fstate).GetDescription())
                ).ForMember(
                    dest => dest.ChannelTitle,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.FchannelTitle))
                );
        }
    }
}