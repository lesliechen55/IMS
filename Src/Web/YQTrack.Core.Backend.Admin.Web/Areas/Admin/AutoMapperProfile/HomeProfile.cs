using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Data.Entity;
using YQTrack.Core.Backend.Admin.DTO.Input;
using YQTrack.Core.Backend.Admin.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.AutoMapperProfile
{
    public class HomeProfile : BaseProfile
    {
        public HomeProfile()
        {
            CreateMap<LoginLogPageDataRequest, LoginLogPageDataInput>();
            CreateMap<OperationLogPageDataRequest, OperationLogPageDataInput>();

            CreateMap<LoginLog, LoginLogPageDataOutput>();
            CreateMap<LoginLogPageDataOutput, LoginLogPageDataResponse>()
                .ForMember(
                    dest => dest.CreatedTime,
                    opt => opt.MapFrom(src => src.FCreatedTime.ToLocalTime())
                ).ForMember(
                    dest => dest.LoginTime,
                    opt => opt.MapFrom(src => src.FLoginTime.ToLocalTime())
                );
            CreateMap<OperationLog, OperationLogPageDataOutput>();
            CreateMap<OperationLogPageDataOutput, OperationLogPageDataResponse>()
                .ForMember(
                    dest => dest.CreatedTime,
                    opt => opt.MapFrom(src => src.FCreatedTime.ToLocalTime())
                ).ForMember(
                    dest => dest.OperationType,
                    opt => opt.MapFrom(src => src.FOperationType.GetDescription())
                );
        }
    }
}