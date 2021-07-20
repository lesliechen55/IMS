using System;
using YQTrack.Backend.Message.Model.Enums;

namespace YQTrack.Core.Backend.Admin.Message.Core
{
    /// <summary>  
    /// 备注特性  
    /// </summary>  
    public class RemarkAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="remark"></param>
        public RemarkAttribute(RabbitMQType remark)
        {
            this.Remark = remark;
        }
        /// <summary>  
        /// 备注  
        /// </summary>  
        public RabbitMQType Remark { get; set; }
    }
}
