using YQTrack.Core.Backend.Admin.Data.Entity;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.AutoMapperProfile
{
    public class ManualReconcileProfile : BaseProfile
    {
        public ManualReconcileProfile()
        {
            CreateMap<ManualReconcile, ManualReconcilePageDataOutput>();
            CreateMap<ManualReconcilePageDataOutput, ManualReconcilePageDataResponse>();

            CreateMap<ManualReconcilePageDataRequest, ManualReconcilePageDataInput>();
        }
    }
}