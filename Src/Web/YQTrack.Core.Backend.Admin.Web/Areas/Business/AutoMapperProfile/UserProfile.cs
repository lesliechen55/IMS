using Newtonsoft.Json;
using System;
using System.Web;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.User.Data.Models;
using YQTrack.Core.Backend.Admin.User.DTO.Input;
using YQTrack.Core.Backend.Admin.User.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Business.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Enums.User;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Business.AutoMapperProfile
{
    public class UserProfile : BaseProfile
    {
        public UserProfile()
        {
            CreateMap<UserPageDataRequest, UserPageDataInput>();

            CreateMap<UserPageDataOutput, UserPageDataResponse>()
                .ForMember(
                    dest => dest.UserRole,
                    opt => opt.MapFrom(src => src.FuserRole.HasValue ? src.FuserRole.Value.GetDescription() : string.Empty)
                    )
                .ForMember(
                    dest => dest.LastSignIn,
                    opt => opt.MapFrom(src => src.FlastSignIn.HasValue ? src.FlastSignIn.Value.ToLocalTime() : (DateTime?)null)
                )
                .ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => src.FcreateTime.HasValue ? src.FcreateTime.Value.ToLocalTime() : (DateTime?)null)
                )
                .ForMember(
                    dest => dest.UpdateTime,
                    opt => opt.MapFrom(src => src.FupdateTime.HasValue ? src.FupdateTime.Value.ToLocalTime() : (DateTime?)null)
                )
                .ForMember(
                    dest => dest.State,
                    opt => opt.MapFrom(src => ((UserState)src.Fstate).GetDescription())
                )
                .ForMember(
                    dest => dest.Source,
                    opt => opt.MapFrom(src => src.Fsource.HasValue ? ((SourceType)src.Fsource.Value).GetDescription() : string.Empty)
                )
                .ForMember(
                    dest => dest.NickName,
                    opt => opt.MapFrom(src => HttpUtility.HtmlEncode(src.FnickName))
                ).ForMember(
                    dest => dest.Country,
                    opt => opt.MapFrom(src => src.Fcountry.HasValue ? LanguageHelper.GetCountryTextZhCn(src.Fcountry.Value.ToString()) : null)
                );

            CreateMap<UserFeedbackPageDataRequest, UserFeedbackPageDataInput>();

            CreateMap<TuserFeedback, UserFeedbackPageDataOutput>();
            CreateMap<UserFeedbackPageDataOutput, UserFeedbackPageDataResponse>()
                .ForMember(
                    dest => dest.State,
                    opt => opt.MapFrom(src => src.Fstate.HasValue ? ((UserFeedbackState)src.Fstate.Value).GetDescription() : string.Empty)
                )
                .ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => src.FcreateTime.HasValue ? src.FcreateTime.Value.ToLocalTime() : (DateTime?)null)
                )
                .ForMember(
                    dest => dest.ReplyTime,
                    opt => opt.MapFrom(src => src.FreplyTime.HasValue ? src.FreplyTime.Value.ToLocalTime() : (DateTime?)null)
                );

            CreateMap<UserDetailOutput, UserDetailResponse>()
                .ForMember(
                dest => dest.UserRole,
                opt => opt.MapFrom(src => src.FuserRole.HasValue ? src.FuserRole.ToString() : string.Empty)
                )
                .ForMember(
                    dest => dest.CreateTime,
                    opt => opt.MapFrom(src => src.FcreateTime.HasValue ? src.FcreateTime.Value.ToLocalTime() : (DateTime?)null)
                )
                .ForMember(
                    dest => dest.Source,
                    opt => opt.MapFrom(src => ((SourceType)src.Fsource).GetDescription())
                )

                .ForMember(
                    dest => dest.LastSignIn,
                    opt => opt.MapFrom(src => src.FlastSignIn.HasValue ? src.FlastSignIn.Value.ToLocalTime() : (DateTime?)null)
                );

            CreateMap<SellerInfoOutput, SellerInfoResponse>()
                .ForMember(
                    dest => dest.UserRoute,
                    opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.UserRoute))
                );

            CreateMap<MemberInfoOutput, MemberInfoResponse>()
                .ForMember(
                dest => dest.MemberLevel,
                opt => opt.MapFrom(src => src.FmemberLevel.ToString())
                ).ForMember(
                    dest => dest.StartTime,
                    opt => opt.MapFrom(src => src.FstartTime.HasValue ? src.FstartTime.Value.ToLocalTime() : (DateTime?)null)
                ).ForMember(
                    dest => dest.ExpiresTime,
                    opt => opt.MapFrom(src => src.FexpiresTime.HasValue ? src.FexpiresTime.Value.ToLocalTime() : (DateTime?)null)
                );

            CreateMap<UserDeviceOutput, UserDeviceResponse>()
                .ForMember(
                    dest => dest.LastVisitTime,
                    opt => opt.MapFrom(src => src.FlastVisitTime.HasValue ? src.FlastVisitTime.Value.ToLocalTime() : (DateTime?)null)
                );
            CreateMap<PaymentOutput, PaymentResponse>()
                .ForMember(
                    dest => dest.CurrencyType,
                    opt => opt.MapFrom(src => src.FCurrencyType.ToString())
                ).ForMember(
                    dest => dest.ProviderId,
                    opt => opt.MapFrom(src => src.FProviderId.ToString())
                ).ForMember(
                    dest => dest.PaymentStatus,
                    opt => opt.MapFrom(src => src.FPaymentStatus.GetDisplayName())
                ).ForMember(
                    dest => dest.CreateAt,
                    opt => opt.MapFrom(src => src.FCreateAt.ToLocalTime())
                )
                .ForMember(
                    dest => dest.UpdateAt,
                    opt => opt.MapFrom(src => src.FUpdateAt.ToLocalTime())
                );

            CreateMap<UserBaseInfoOutput, UserBaseInfoResponse>();
        }
    }
}