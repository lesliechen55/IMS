using YQTrack.Core.Backend.Admin.TrackApi.DTO.Input;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;
using YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.AutoMapperProfile
{
    public class UserInvoiceProfile : BaseProfile
    {
        public UserInvoiceProfile()
        {
            CreateMap<UserInvoiceRequest, UserInvoiceInput>();
            CreateMap<UserInvoiceOutput, UserInvoiceResponse>();
            //CreateMap<UserInvoiceOutput, UserInvoiceResponse>().ForMember(
            //        dest => dest.Amount,
            //        opt => opt.MapFrom(src =>
            //        {
            //            string cny = src.FCNYAmount == 0 ? "" : $"CNY ￥{src.FCNYAmount}";
            //            string usd = src.FUSDAmount == 0 ? "" : string.IsNullOrWhiteSpace(cny) ? "" : " " + $"USD ${src.FUSDAmount}";
            //            return string.IsNullOrWhiteSpace(cny) && string.IsNullOrWhiteSpace(usd) ? "--" : (cny + usd);
            //        })
            //    );
        }
    }
}
