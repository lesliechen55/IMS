using System;
using System.Web;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Freight.Data.Models;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Freight.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Freight.AutoMapperProfile
{
    public class InquiryProfile : BaseProfile
    {
        public InquiryProfile()
        {
            CreateMap<TinquiryOrder, InquiryPageDataOutput>();
            CreateMap<InquiryPageDataOutput, InquiryPageDataResponse>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.ForderId)
                ).ForMember(
                    dest => dest.InquiryNo,
                    opt => opt.MapFrom(src => src.FinquiryOrderNo)
                ).ForMember(
                    dest => dest.PublishDateTime,
                    opt => opt.MapFrom(src => src.FcreateTime.ToLocalTime())
                ).ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => ((InquiryOrderStatus)src.Fstatus).GetDescription())
                ).ForMember(
                    dest => dest.ProcessTime,
                    opt => opt.MapFrom(src => src.FprocessTime.HasValue ? src.FprocessTime.Value.ToLocalTime() : (DateTime?)null)
                ).ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.Ftitle))
                ).ForMember(
                    dest => dest.ExpireDate,
                    opt => opt.MapFrom(src => src.FexpireDate.ToLocalTime())
                ).ForMember(
                    dest => dest.StatusTime,
                    opt => opt.MapFrom(src => src.FstatusTime.ToLocalTime())
                ).ForMember(
                    dest => dest.ContactInfo,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.FcontactInfo))
                );

            CreateMap<InquiryOrderStatusLogPageDataRequest, InquiryOrderStatusLogPageDataInput>();
            CreateMap<TInquiryOrderStatusLog, InquiryOrderStatusLogPageDataOutput>();
            CreateMap<InquiryOrderStatusLogPageDataOutput, InquiryOrderStatusLogPageDataResponse>()
                .ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => src.FCreateTime.ToLocalTime())
                )
                .ForMember(
                    dest => dest.From,
                    opt => opt.MapFrom(src => src.FFrom.GetDescription())
                )
                .ForMember(
                    dest => dest.To,
                    opt => opt.MapFrom(src => src.FTo.GetDescription())
                );
        }
    }
}