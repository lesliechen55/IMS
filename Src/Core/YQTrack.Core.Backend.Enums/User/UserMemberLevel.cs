using System;
using System.Linq;

namespace YQTrack.Core.Backend.Enums.User
{
    /// <summary>
    /// 会员等级枚举定义
    /// </summary>
    public enum UserMemberLevel
    {
        /// <summary>
        /// 无
        /// </summary>
        FreeMember = 0,

        /// <summary>
        /// 普通会员
        /// </summary>
        [AttUserMemberType(UserMemberType.BuyerMemberType)]
        BuyerOrdinaryMember = 4010,
        /// <summary>
        /// 高级会员
        /// </summary>
        [AttUserMemberType(UserMemberType.BuyerMemberType)]
        BuyerSeniorMember = 4020,



        /// <summary>
        /// 普通会员
        /// </summary>
        [AttUserMemberType(UserMemberType.SellerMemberType)]
        SellerOrdinaryMember = 2010,

        /// <summary>
        /// 高级会员
        /// </summary>
        [AttUserMemberType(UserMemberType.SellerMemberType)]
        SellerSeniorMember = 2020,

    }


    /// <summary>
    ///  会员类型
    /// </summary>
    public enum UserMemberType
    {
        /// <summary>
        /// 无
        /// </summary>
        None=0,
        /// <summary>
        /// 
        /// </summary>
         SellerMemberType=20,
        /// <summary>
        /// 
        /// </summary>
         BuyerMemberType = 40,
    }


    /// <summary>
    /// 用户会员类型属性标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class AttUserMemberTypeAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberType"></param>
        public AttUserMemberTypeAttribute(UserMemberType memberType)
        {
            MemberType = memberType;
        }

        /// <summary>
        /// 会员类型
        /// </summary>
        public UserMemberType MemberType { get; private set; }
    }

    /// <summary>
    /// 用户会员类型属性标记 帮助类
    /// </summary>
    public sealed class AttUserMemberTypeHelper
    {

        /// <summary>
        /// 获取会员等级上标记属性（AttUserMemberTypeAttribute）的会员类型
        /// </summary>
        /// <param name="memberLevel"></param>
        /// <returns></returns>
        public static UserMemberType GetUserMemberTypeByAttribute(UserMemberLevel memberLevel)
        {
            Type enumType = memberLevel.GetType();
            Type attType = typeof(AttUserMemberTypeAttribute);
            var strFieldName = Enum.GetName(enumType, memberLevel);
            if (!string.IsNullOrWhiteSpace(strFieldName))
            {
                var field = enumType.GetField(strFieldName);
                var attributes = field.GetCustomAttributes(attType, false).OfType<AttUserMemberTypeAttribute>().ToList();
                if (attributes.Count == 0)
                {
                    return UserMemberType.None;
                }
                else
                {
                    return attributes[0].MemberType;
                }
            }
            return UserMemberType.None;
        }
    }

}
