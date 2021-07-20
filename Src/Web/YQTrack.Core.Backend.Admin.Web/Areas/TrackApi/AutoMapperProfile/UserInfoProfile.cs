using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Input;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.TrackApi;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.AutoMapperProfile
{
    public class UserInfoProfile : BaseProfile
    {
        public UserInfoProfile()
        {
            CreateMap<UserInfoEditRequest, UserInfoEditInput>();
            CreateMap<ChangeApiStateRequest, ChangeApiStateInput>();

            CreateMap<UserInfoPageDataOutput, UserInfoPageDataResponse>()
                .ForMember(
                    dest => dest.CreatedTime,
                    opt => opt.MapFrom(src => src.FCreatedTime.ToLocalTime())
                ).ForMember(
                    dest => dest.ScheduleFrequency,
                    opt => opt.MapFrom(src => src.FScheduleFrequency.HasValue && Enum.IsDefined(typeof(ScheduleFrequency), src.FScheduleFrequency.Value) ? ((ScheduleFrequency)src.FScheduleFrequency.Value).GetDescription() : "暂无配置")
                ).ForMember(
                    dest => dest.GiftQuota,
                    opt => opt.MapFrom(src => src.FGiftQuota.HasValue ? src.FGiftQuota.Value.ToString("N0") : "暂无配置")
                ).ForMember(
                    dest => dest.Remain,
                    opt => opt.MapFrom(src => src.FRemain.ToString("N0"))
                ).ForMember(
                    dest => dest.TodayUsed,
                    opt => opt.MapFrom(src => src.FTodayUsed.ToString("N0"))
                );

            CreateMap<UserInfoOutput, UserInfoResponse>().ForMember(
                    dest => dest.CreatedTime,
                    opt => opt.MapFrom(src => src.FCreatedTime.ToLocalTime())
                ).ForMember(
                    dest => dest.ApiState,
                    opt => opt.MapFrom(src => ((ApiState)src.FApiState).GetDescription())
                ).ForMember(
                dest => dest.ScheduleFrequency,
                opt => opt.MapFrom(src => ((ScheduleFrequency)src.FScheduleFrequency))
            );
            CreateMap<UserInfoViewOutput, UserInfoViewResponse>();

            CreateMap<UserConfigOutput, UserConfigResponse>()
                .ForMember(
                    dest => dest.ScheduleFrequency,
                    opt => opt.MapFrom(src => src.FScheduleFrequency.GetDescription())
                )
                .ForMember(
                    dest => dest.AppSecretKey,
                    opt => opt.MapFrom(src => AppSecretKeyHelper.GetSecretKey(src.FUserNo,src.FSecretSeed))
                ).ForMember(
                    dest => dest.IPWhiteList,
                    opt => opt.MapFrom(src => src.FIPWhiteList.IsNotNullOrWhiteSpace() ? JsonConvert.DeserializeObject<ICollection<string>>(src.FIPWhiteList) : new System.Collections.ObjectModel.Collection<string>())
                ).ForMember(
                    dest => dest.ApiNotify,
                    opt => opt.MapFrom(src => src.FApiNotify.IsNotNullOrWhiteSpace() ? JsonConvert.DeserializeObject<ApiNotify>(src.FApiNotify) : null)
                );
        }
    }
}
