using System;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Data.Entity;
using YQTrack.Core.Backend.Admin.DTO.Input;
using YQTrack.Core.Backend.Admin.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Admin.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Admin.AutoMapperProfile
{
    public class PermissionProfile : BaseProfile
    {
        public PermissionProfile()
        {
            // 输出
            CreateMap<Permission, PermissionOutput>();
            CreateMap<PermissionOutput, PermissionResponse>().ForMember(
                dest => dest.ParentId,
                opt => opt.MapFrom(src => src.FParentId ?? -1)
            ).ForMember(
                dest => dest.CreatedTime,
                opt => opt.MapFrom(src => src.FCreatedTime.ToLocalTime())
            ).ForMember(
                dest => dest.UpdatedTime,
                opt => opt.MapFrom(src => src.FUpdatedTime.HasValue ? src.FUpdatedTime.Value.ToLocalTime() : (DateTime?)null)
            ).ForMember(
                dest => dest.MenuTypeDesc,
                opt => opt.MapFrom(src => src.FMenuType.GetDescription())
            );

            // 输入
            CreateMap<PermissionAddRequest, PermissionAddInput>();
            CreateMap<PermissionAddInput, Permission>();

        }
    }
}