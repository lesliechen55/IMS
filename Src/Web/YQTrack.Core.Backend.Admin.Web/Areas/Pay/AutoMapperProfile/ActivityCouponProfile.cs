using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.Data.Models;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
{
    public class ActivityCouponProfile : BaseProfile
    {
        public ActivityCouponProfile()
        {
            CreateMap<ActivityCouponEditRequest, ActivityCouponEditInput>();
            CreateMap<ActivityCouponEditInput, TActivityCoupon>();
            
            CreateMap<ActivityCouponPageDataRequest, ActivityCouponPageDataInput>();
            CreateMap<ActivityCouponPageDataOutput, ActivityCouponPageDataResponse>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => (src.FStatus).GetDescription()))
                //.ForMember(
                //    dest => dest.CreateAt,
                //    opt => opt.MapFrom(src => src.CreateAt.ToLocalTime())
                //).ForMember(
                //    dest => dest.UpdateAt,
                //    opt => opt.MapFrom(src => src.UpdateAt.HasValue ? src.UpdateAt.Value.ToLocalTime() : (DateTime?)null)
                //)
            ;
        }
    }
}
