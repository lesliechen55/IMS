namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request
{
    public class IndexPageDataRequest //: PageRequest
    {
        //[Required(AllowEmptyStrings = false)]
        public string Email { get; set; }

        /// <summary>
        /// 表示用户是否启用状态
        /// </summary>
        public bool? Enable { get; set; }

        public int? OfflineDay { get; set; }
    }
}