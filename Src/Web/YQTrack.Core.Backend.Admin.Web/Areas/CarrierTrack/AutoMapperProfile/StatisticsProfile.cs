using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Input;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.AutoMapperProfile
{
    public class StatisticsProfile : BaseProfile
    {
        public StatisticsProfile()
        {
            CreateMap<UserMarkLogPageDataRequest, UserMarkLogPageDataInput>();

            CreateMap<UserMarkLogPageDataOutput, UserMarkLogPageDataResponse>();

            CreateMap<ExportUserMarkLogOutput, ExportUserMarkLogResponse>();
        }
    }
}