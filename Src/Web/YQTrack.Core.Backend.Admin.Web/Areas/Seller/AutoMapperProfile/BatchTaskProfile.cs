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
    public class BatchTaskProfile : BaseProfile
    {
        public BatchTaskProfile()
        {
            CreateMap<BatchTaskPageDataRequest, BatchTaskPageDataInput>()
            .ForMember(
                    dest => dest.UserRoute,
                    opt => opt.MapFrom(src => JsonConvert.DeserializeObject<UserRouteDto>(src.UserRoute))
                );
            CreateMap<BatchTaskPageDataOutput, BatchTaskPageDataResponse>()
                .ForMember(
                    dest => dest.TaskStartTime,
                    opt => opt.MapFrom(src => src.FTaskStartTime.HasValue ? src.FTaskStartTime.Value.ToLocalTime() : (DateTime?)null)
                )
                .ForMember(
                    dest => dest.TaskEndTime,
                    opt => opt.MapFrom(src => src.FTaskEndTime.HasValue ? src.FTaskEndTime.Value.ToLocalTime() : (DateTime?)null)
                )
                .ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => src.FCreateTime.ToLocalTime())
                )
                .ForMember(
                    dest => dest.TaskType,
                    opt => opt.MapFrom(src => LanguageHelper.GetBatchTaskTypeText(src.FTaskType))
                )
                .ForMember(
                    dest => dest.TaskStatus,
                    opt => opt.MapFrom(src => src.FTaskStatus.GetDescription())
                )
                .ForMember(
                    dest => dest.Total,
                    opt => opt.MapFrom(src => src.FTotal.ToString("N0"))
                )
                .ForMember(
                    dest => dest.Success,
                    opt => opt.MapFrom(src => src.FSuccess.ToString("N0"))
                )
                .ForMember(
                    dest => dest.Error,
                    opt => opt.MapFrom(src => src.FError.ToString("N0"))
                );
        }
    }
}
