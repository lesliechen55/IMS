using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YQTrack.Core.Backend.Admin.Web.Areas.RealTime.Models.Response
{
    public class PlatformShopDataResponse
    {
        /// <summary>
        /// 平台类型
        /// </summary>
        public string PlatformType { get; set; }

        /// <summary>
        /// 平台名称
        /// </summary>
        public string PlatformTypeName { get; set; }

        /// <summary>
        /// 平台店铺总数量
        /// </summary>
        public int SumCount { get; set; }
    }
}
