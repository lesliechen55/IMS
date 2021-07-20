using System;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp.Dto
{
    /// <summary>
    /// 订单号生成实体
    /// </summary>
    public class OrderSerialNoDto
    {
        public int FNo { get; set; }

        public DateTime FUpdateAt { get; set; }

        public bool FIsProdEnv { get; set; }

        public int EnvNo
        {
            get
            {
                return FIsProdEnv ? 1 : 0;
            }
        }
    }
}
