using YQTrack.Core.Backend.Admin.Data.Entity;
using YQTrack.Core.Backend.Admin.DTO.Input;
using YQTrack.Core.Backend.Admin.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.AutoMapperProfile
{
    public class ManagerProfile : BaseProfile
    {
        public ManagerProfile()
        {
            // 输出
            CreateMap<Manager, ManagerPageDataOutput>();
            CreateMap<ManagerPageDataOutput, ManagerPageDataResponse>().ForMember(
                dest => dest.CreatedTime,
                opt => opt.MapFrom(src => src.FCreatedTime.ToLocalTime())
                );
            CreateMap<ManagerRoleOutput, ManagerRoleResponse>();

            // 输入
            CreateMap<ManagerPageDataRequest, ManagerPageDataInput>();
            CreateMap<ManagerAddRequest, ManagerAddInput>();
            CreateMap<ManagerAddInput, Manager>();

        }
    }
}