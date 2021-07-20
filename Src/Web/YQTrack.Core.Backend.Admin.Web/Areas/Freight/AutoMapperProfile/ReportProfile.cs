using System;
using System.Web;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.AutoMapperProfile
{
    public class ReportProfile : BaseProfile
    {
        public ReportProfile()
        {
            CreateMap<ReportPageDataRequest, ReportPageDataInput>();

            CreateMap<ReportPageDataOutput, ReportPageDataResponse>()
                .ForMember(
                    dest => dest.ChannelTitle,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.FChannelTitle))
                    )
                .ForMember(
                    dest => dest.CompanyName,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.FCompanyName))
                )
                .ForMember(
                    dest => dest.Detail,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.FDetail))
                )
                .ForMember(
                    dest => dest.ReasonType,
                    opt => opt.MapFrom(src => ((ReportReasonEnum)src.FReasonType).GetDescription())
                )
                .ForMember(
                    dest => dest.ReportTime,
                    opt => opt.MapFrom(src => src.FReportTime.ToLocalTime())
                )
                .ForMember(
                    dest => dest.ProcessStatus,
                    opt => opt.MapFrom(src => ((ProcessReportStatusEnum)src.FProcessStatus).GetDescription())
                )
                .ForMember(
                    dest => dest.ProcessTime,
                    opt => opt.MapFrom(src => src.FProcessTime.HasValue ? src.FProcessTime.Value.ToLocalTime() : (DateTime?)null)
                );
        }
    }
}