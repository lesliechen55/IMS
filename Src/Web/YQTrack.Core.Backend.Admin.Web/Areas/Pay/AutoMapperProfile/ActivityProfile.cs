using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
{
    public class ActivityProfile : BaseProfile
    {
        public ActivityProfile()
        {
            CreateMap<ActivityEditRequest, ActivityEditInput>();
            CreateMap<ActivityEditInput, TActivity>();
            //
            CreateMap<ActivityPageDataRequest, ActivityPageDataInput>();
            CreateMap<ActivityPageDataOutput, ActivityPageDataResponse>()
                .ForMember(
                    dest => dest.ActivityType,
                    opt => opt.MapFrom(src => (src.FActivityType).GetDescription()))
                .ForMember(
                    dest => dest.CouponMode,
                    opt => opt.MapFrom(src => (src.FCouponMode).GetDescription()))
                .ForMember(
                    dest => dest.DiscountType,
                    opt => opt.MapFrom(src => (src.FDiscountType).GetDescription()))
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => (src.FStatus).GetDescription()));
        }
    }
}