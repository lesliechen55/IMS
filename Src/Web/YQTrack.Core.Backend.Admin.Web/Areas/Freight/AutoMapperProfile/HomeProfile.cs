using System;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.AutoMapperProfile
{
    public class HomeProfile : BaseProfile
    {
        public HomeProfile()
        {
            CreateMap<QuotePageDataRequest, QuotePageDataInput>();

            CreateMap<QuotePageDataOutput, QuotePageDataResponse>()
                .ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => src.FcreateTime.ToLocalTime())
                    )
                .ForMember(
                    dest => dest.CancelTime,
                    opt => opt.MapFrom(src => src.FcancelTime.HasValue ? src.FcancelTime.Value.ToLocalTime() : (DateTime?)null)
                ).ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => ((InquiryOrderStatus)src.Fstatus).GetDescription())
                    );
        }
    }
}