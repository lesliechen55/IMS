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
    public class CompanyProfile : BaseProfile
    {
        public CompanyProfile()
        {


            CreateMap<CompanyPageDataRequest, CompanyPageDataInput>();

            CreateMap<Tcompany, CompanyPageDataOutput>();
            CreateMap<CompanyPageDataOutput, CompanyPageDataResponse>()
                .ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => src.FcreateTime.ToLocalTime())
                )
                .ForMember(
                    dest => dest.UpdateTime,
                    opt => opt.MapFrom(src => src.FupdateTime.ToLocalTime())
                )
                .ForMember(
                    dest => dest.CheckState,
                    opt => opt.MapFrom(src => ((CompanyAuditState)src.FcheckState).GetDescription())
                )
                .ForMember(
                    dest => dest.CompanyName,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.FcompanyName))
                )
                .ForMember(
                    dest => dest.Address,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.Faddress))
                )
                .ForMember(
                    dest => dest.Contact,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.Fcontact))
                )
                .ForMember(
                    dest => dest.Email,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.Femail))
                )
                .ForMember(
                    dest => dest.Mobile,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.Fmobile))
                )
                .ForMember(
                    dest => dest.Url,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.Furl))
                );

            CreateMap<CompanyEditOutput, CompanyEditResponse>();
            CreateMap<CompanyEditRequest, CompanyEditInput>();

            CreateMap<Tcompany, CompanyViewCheckOutput>();
            CreateMap<CompanyViewCheckOutput, CompanyViewCheckResponse>()
                .ForMember(
                    dest => dest.Scale,
                    opt => opt.MapFrom(src => GetScaleDesc(src.Fscale))
                    );
        }

        private static string GetScaleDesc(int scale)
        {
            switch (scale)
            {
                case 0:
                    return "<30人";
                case 1:
                    return "30-100人";
                case 2:
                    return "100-300人";
                case 3:
                    return ">300人";
                default:
                    return string.Empty;
            }
        }
    }
}