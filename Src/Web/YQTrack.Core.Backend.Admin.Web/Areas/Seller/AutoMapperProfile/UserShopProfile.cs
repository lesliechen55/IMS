using Newtonsoft.Json;
using System;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Seller.DTO.Input;
using YQTrack.Core.Backend.Admin.Seller.DTO.Output;
using YQTrack.Core.Backend.Admin.User.DTO;
using YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Seller.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Seller.AutoMapperProfile
{
    public class UserShopProfile : BaseProfile
    {
        public UserShopProfile()
        {
            CreateMap<UserShopPageDataRequest, UserShopPageDataInput>()
                .ForMember(
                    dest => dest.UserRoute,
                    opt => opt.MapFrom(src => JsonConvert.DeserializeObject<UserRouteDto>(src.UserRoute))
                );
            CreateMap<UserShopPageDataOutput, UserShopPageDataResponse>()
                .ForMember(
                    dest => dest.LastSyncTime,
                    opt => opt.MapFrom(src => src.FLastSyncTime.HasValue ? src.FLastSyncTime.Value.ToLocalTime() : (DateTime?)null)
                )
                .ForMember(
                    dest => dest.NextSyncTime,
                    opt => opt.MapFrom(src => src.FNextSyncTime.HasValue ? src.FNextSyncTime.Value.ToLocalTime() : (DateTime?)null)
                )
                .ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => src.FCreateTime.ToLocalTime())
                )
                .ForMember(
                    dest => dest.PlatformType,
                    opt => opt.MapFrom(src => LanguageHelper.GetPlatformText(src.FPlatformType))
                )
                .ForMember(
                    dest => dest.State,
                    opt => opt.MapFrom(src => src.FState.GetDescription())
                )
                .ForMember(
                    dest => dest.LastSyncNum,
                    opt => opt.MapFrom(src => src.FLastSyncNum.ToString("N0"))
                );
            CreateMap<TrackUploadRecordRequest, TrackUploadRecordInput>()
               .ForMember(
                   dest => dest.UserRoute,
                   opt => opt.MapFrom(src => JsonConvert.DeserializeObject<UserRouteDto>(src.UserRoute))
               );

            CreateMap<TrackUploadRecordOutput, TrackUploadRecordResponse>()
                .ForMember(
                    dest => dest.RowTotal,
                    opt => opt.MapFrom(src => src.FRowTotal.ToString("N0"))
                )
                .ForMember(
                    dest => dest.SuccessTotal,
                    opt => opt.MapFrom(src => src.FSuccessTotal.ToString("N0"))
                )
                .ForMember(
                    dest => dest.ErrorTotal,
                    opt => opt.MapFrom(src => src.FErrorTotal.ToString("N0"))
                );
        }
    }
}
