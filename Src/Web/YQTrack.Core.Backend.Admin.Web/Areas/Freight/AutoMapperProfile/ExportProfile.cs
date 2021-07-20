using YQTrack.Core.Backend.Admin.Freight.Data.Models;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.AutoMapperProfile
{
    public class ExportProfile : BaseProfile
    {
        public ExportProfile()
        {
            CreateMap<Tcompany, ExportCarrierOutput>();
            CreateMap<ExportCarrierOutput, ExportCarrierResponse>();

            CreateMap<ExportValidChannelOutput, ExportValidChannelResponse>()
                .ForMember(
                    dest => dest.FpublishTime,
                    opt => opt.MapFrom(src => src.FpublishTime.HasValue ? src.FpublishTime.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : string.Empty)
                    );

            CreateMap<ExportInvalidChannelOutput, ExportInvalidChannelResponse>()
                .ForMember(
                    dest => dest.FpublishTime,
                    opt => opt.MapFrom(src => src.FpublishTime.HasValue ? src.FpublishTime.Value.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss") : string.Empty)
                    )
                .ForMember(
                    dest => dest.FexpireTime,
                    opt => opt.MapFrom(src => src.FexpireTime.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"))
                    );
        }
    }
}