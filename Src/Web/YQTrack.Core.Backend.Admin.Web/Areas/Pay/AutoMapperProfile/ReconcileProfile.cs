using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
{
    public class ReconcileProfile : BaseProfile
    {
        public ReconcileProfile()
        {
            CreateMap<ReconcilePageDataOutput, ReconcilePageDataResponse>().ForMember(
                    dest => dest.BeginTime,
                    opt => opt.MapFrom(src => src.FBeginTime.ToLocalTime())
                )
                .ForMember(
                    dest => dest.ProviderId,
                    opt => opt.MapFrom(src => src.FProviderId.GetDescription())
                );
            CreateMap<ReconcileItemShowOutput, ReconcileItemShowResponse>()
                .ForMember(
                    dest => dest.Status,
                    opt => opt.MapFrom(src => src.FStatus.GetDisplayName())
                )
                .ForMember(
                    dest => dest.ReconcileState,
                    opt => opt.MapFrom(src => src.FReconcileState.ToString())
                )
                .ForMember(
                    dest => dest.ProviderId,
                    opt => { opt.MapFrom(src => src.FProviderId.GetDescription()); }
                );
        }
    }
}
