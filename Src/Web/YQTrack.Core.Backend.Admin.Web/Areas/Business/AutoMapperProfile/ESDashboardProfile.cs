using Newtonsoft.Json;
using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Data.Entity;
using YQTrack.Core.Backend.Admin.DTO;
using YQTrack.Core.Backend.Admin.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.AutoMapperProfile
{
    public class ESDashboardProfile : BaseProfile
    {
        public ESDashboardProfile()
        {

            CreateMap<ESDashboard, ESDashboardDto>()
                .ForMember(
                    dest => dest.FFieldsConfig,
                    opt => opt.MapFrom(src => src.FFieldsConfig.IsNullOrWhiteSpace() ? new List<ESFieldsConfigDto>() : JsonConvert.DeserializeObject<List<ESFieldsConfigDto>>(src.FFieldsConfig))
                );


            CreateMap<ESFieldsConfigRequest, ESFieldsConfigDto>();

            CreateMap<ESDashboardEditRequest, ESDashboardDto>();

            CreateMap<ESDashboardDto, ESDashboard>()
                .ForMember(
                    dest => dest.FFieldsConfig,
                    opt => opt.MapFrom(src => JsonHelper.ToJson(src.FFieldsConfig))
                );

            CreateMap<ESDashboardDto, ESDashboardResponse>()
                 .ForMember(
                    dest => dest.FieldsConfig,
                    opt => opt.MapFrom(src => JsonHelper.ToJson(src.FFieldsConfig))
                );

            CreateMap<ESFieldOutput, ESFieldResponse>();

            CreateMap<ESDashboardDetailOutput, ESDashboardDetailResponse>();
        }
    }
}