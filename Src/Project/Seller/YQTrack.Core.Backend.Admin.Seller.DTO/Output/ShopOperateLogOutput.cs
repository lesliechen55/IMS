using System;

namespace YQTrack.Core.Backend.Admin.Seller.DTO.Output
{
    public class ShopOperateLogOutput
    {
        /// <summary>
        /// 操作日志类型
        /// </summary>
        public int FOperateType { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime FCreateTime { get; set; }

        /// <summary>
        /// Json格式的数据
        /// </summary>
        public string FLogData { set; get; }
    }
}
