using YQTrack.Core.Backend.Admin.Data.Entity;
using YQTrack.Core.Backend.Admin.DTO.Input;
using YQTrack.Core.Backend.Admin.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.AutoMapperProfile
{
    public class RoleProfile : BaseProfile
    {
        public RoleProfile()
        {
            // 输出
            CreateMap<Role, RolePageDataOutput>();
            CreateMap<RolePageDataOutput, RolePageDataResponse>().ForMember(
                dest => dest.CreatedTime,
                opt => opt.MapFrom(src => src.FCreatedTime.ToLocalTime())
                );

            // 输入
            CreateMap<RoleAddRequest, RoleAddInput>();
            CreateMap<RoleAddInput, Role>();

            CreateMap<Permission, RolePermissionOutput>();
            CreateMap<RolePermissionOutput, RolePermissionResponse>();

        }
    }
}