using System;
using YQTrack.Core.Backend.Enums.Seller;

namespace YQTrack.Core.Backend.Admin.RealTime.Data.Models
{
    public partial class TUserShop
    {
        public long FShopId { get; set; }
        public string FShopName { get; set; }
        public string FShopAlias { get; set; }
        public long FUserId { get; set; }
        public int FPlatformType { get; set; }
        public string FPlatformUID { get; set; }
        public string FPlatformArgs { get; set; }
        public string FAccessToken { get; set; }
        public ShopStateType? FState { get; set; }
        public DateTime? FTokenExpire { get; set; }
        public DateTime? FLastSyncTime { get; set; }
        public int? FLastSyncNum { get; set; }
        public DateTime FCreateTime { get; set; }
        public long FCreateBy { get; set; }
        public DateTime FUpdateTime { get; set; }
        public long FUpdateBy { get; set; }
        public byte[] FTimeStamp { get; set; }
        public DateTime? FNextSyncTime { get; set; }
        public bool FThreadId { get; set; }
        public int? FSyncTimeSpan { get; set; }
        public bool? FHasOrder { get; set; }
        public bool? FIsDefault { get; set; }
        public bool? FIsSendEmail { get; set; }
        public DateTime? FSyncTime { get; set; }
        public DateTime? FImportTime { get; set; }
        public bool? FIsImporting { get; set; }
        public int FImportNum { get; set; }
        public DateTime? FLastImportedTime { get; set; }
        public DateTime? FLastSyncedTime { get; set; }
        public DateTime? FNextMessageSyncingTime { get; set; }
        public string FShopEmail { get; set; }
    }
}
