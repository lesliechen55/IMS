using AutoMapper;
using YQTrack.Core.Backend.Admin.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Models.Response;

namespace YQTrack.Core.Backend.Admin.Web.AutoMapperProfile
{
    public class HomeProfile : Profile
    {
        public HomeProfile()
        {
            CreateMap<MenuOutput, MenuResponse>();
        }
    }
}