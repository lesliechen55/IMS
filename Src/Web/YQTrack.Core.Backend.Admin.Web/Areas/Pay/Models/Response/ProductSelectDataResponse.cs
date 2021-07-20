using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;
using YQTrack.Backend.Payment.Model.Enums;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.Web.Areas.Pay.Models.Response
{
    public class ProductSelectDataResponse
    {
        /// <summary>
        /// 角色类型
        /// </summary>
        public Dictionary<int, string> ListRoleType => EnumHelper.GetSelectItem<UserRoleType>();

        /// <summary>
        /// 商品服务类型
        /// </summary>
        public List<ProductServiceType> ListServiceType
        {
            get
            {
                var list = new List<ProductServiceType>();
                foreach (ServiceType item in Enum.GetValues(typeof(ServiceType)))
                {
                    if ((int)item != 0)
                    {
                        list.Add(new ProductServiceType()
                        {
                            ServiceId = (int)item,
                            ServiceName = item.GetDescription()
                        });
                    }
                }
                return list;
            }
        }
        /// <summary>
        /// 商品类别
        /// </summary>
        public List<ProductCategoryOutput> ListCategory { get; set; }
    }
    /// <summary>
    /// 角色类型
    /// </summary>
    public class RoleType
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
    }
    /// <summary>
    /// 商品服务类型
    /// </summary>
    public class ProductServiceType
    {
        /// <summary>
        /// 商品服务ID
        /// </summary>
        public int ServiceId { get; set; }

        /// <summary>
        /// 商品服务名称
        /// </summary>
        public string ServiceName { get; set; }
    }
}
