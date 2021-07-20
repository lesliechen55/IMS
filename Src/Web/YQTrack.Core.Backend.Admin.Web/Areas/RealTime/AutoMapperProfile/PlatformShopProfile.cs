using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.RealTime.DTO.Input;
using YQTrack.Core.Backend.Admin.RealTime.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.RealTime.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.RealTime.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.RealTime.AutoMapperProfile
{
    public class PlatformShopProfile : BaseProfile
    {
        public PlatformShopProfile()
        {
            CreateMap<PlatformShopDataRequest, PlatformShopDataInput>();
            CreateMap<PlatformShopDataOutput, PlatformShopDataResponse>()
                .ForMember(
                    dest => dest.PlatformTypeName,
                    opt => opt.MapFrom(src => GetEnumName(src.PlatformType))
                );
        }

        private string GetEnumName(int i)
        {
            try
            {
                string str = Enum.GetName(typeof(YQTrack.Backend.ThirdPlatform.Enums.PlatformType), i);
                return str;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
