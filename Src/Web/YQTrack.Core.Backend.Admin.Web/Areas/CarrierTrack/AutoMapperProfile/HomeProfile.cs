using System;
using YQTrack.Core.Backend.Admin.CarrierTrack.Data.Models;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Input;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.AutoMapperProfile
{
    public class HomeProfile : BaseProfile
    {
        public HomeProfile()
        {
            CreateMap<IndexPageDataRequest, IndexPageDataInput>();
            CreateMap<TControl, IndexPageDataOutput>();
            CreateMap<IndexPageDataOutput, IndexPageDataResponse>().ForMember(
                dest => dest.CreateTime,
                opt => opt.MapFrom(src => src.FCreateTime.ToLocalTime())
            ).ForMember(
                dest => dest.UpdateTime,
                opt => opt.MapFrom(src => src.FUpdateTime.HasValue ? src.FUpdateTime.Value.ToLocalTime() : (DateTime?)null)
            ).ForMember(
                dest => dest.OfflineDay,
                opt => opt.MapFrom(src => src.FLastAccessTime.HasValue ? (DateTime.UtcNow - src.FLastAccessTime.Value).Days : (int?)null)
            );

            CreateMap<CarrierTrackUserAddRequest, CarrierTrackUserAddInput>();
            CreateMap<CarrierTrackUserAddInput, TControl>();
        }
    }
}