using System;
using System.Text.RegularExpressions;

namespace YQTrack.Core.Backend.Admin.Core
{
    public static class ValidatorExtension
    {
        /// <summary>
        /// 验证正整数(非0开头)
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns>验证结果</returns>
        public static bool IsInt(this string num, bool allowNull)
        {
            return IsMatchRegEx(num, allowNull, @"^(?!(0\d*$))\d+$");
        }

        /// <summary>
        /// 验证正浮点数(保留两位小数)
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns>验证结果</returns>
        public static bool IsFloat(this string num, bool allowNull)
        {
            return IsMatchRegEx(num, allowNull, @"^(?!(0\d*$))\d+\.?\d{0,2}$");
        }

        /// <summary>
        /// 验证QQ格式
        /// </summary>
        /// <param name="qq">邮箱</param>
        /// <param name="allowNull">是否允许空值</param>
        /// <returns>验证结果</returns>
        public static bool IsQQ(this string qq, bool allowNull)
        {
            return IsMatchRegEx(qq, allowNull, @"\d{5,11}");
        }

        /// <summary>
        /// 验证邮箱格式
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <param name="allowNull">是否允许空值</param>
        /// <returns>验证结果</returns>
        public static bool IsEmail(this string email, bool allowNull)
        {//^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$
            return IsMatchRegEx(email, allowNull, @"^((?=[a-zA-Z0-9])[a-zA-Z0-9._-]+)?[a-zA-Z0-9]{1}@[a-zA-Z0-9]+(-?[a-zA-Z0-9]+)?(\.[a-zA-Z0-9]+)*(\.[a-zA-Z]{2,}){1}$");
        }

        /// <summary>
        /// 验证手机格式
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <param name="allowNull">是否允许空值</param>
        /// <returns>验证结果</returns>
        public static bool IsPhone(this string phone, bool allowNull)
        {
            //中国地区手机正则：^13[0-9][0-9]{8}|14[57][0-9]{8}|15[012356789][0-9]{8}|18[0-9][0-9]{8}|17[678][0-9]{8}
            return IsMatchRegEx(phone, allowNull, @"^13[0-9]{9}$|14[0-9]{9}|15[0-9]{9}$|18[0-9]{9}$");
        }
        /// <summary>
        /// 验证账号格式
        /// </summary>
        /// <param name="userName">账号</param>
        /// <returns>验证结果</returns>
        public static bool IsUserName(this string userName)
        {
            return IsMatchRegEx(userName, false, @"[A-Za-z0-9_]{6,16}");
        }
        /// <summary>
        /// 验证资金密码格式
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns>验证结果</returns>
        public static bool IsTradePwd(this string pwd)
        {
            return IsMatchRegEx(pwd, false, @"[A-Za-z0-9]{8,16}");
        }
        /// <summary>
        /// 验证密码格式
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns>验证结果</returns>
        public static bool IsPwd(this string pwd, bool allowNull)
        {
            //[A-Za-z0-9_\~\!\@\#\$\%\^\&\*\(\)\-\+\=\[\]\{\}\<\>\?\:\;]{6,16}
            //限制不能纯数字或纯字母或纯符号/必须数字字母符号三种中的2种
            return IsMatchRegEx(pwd, allowNull, @"^(?!^\d+$)(?!^[a-zA-Z]+$)(?!^[\~\`\!\@\#\$\%\^\&\*\(\)\-\+\=\{\[\}\]\|\\\:\;\”\’\<\,\>\.\?\/]+$)[\w\~\`\!\@\#\$\%\^\&\*\(\)\-\+\=\{\[\}\]\|\\\:\;\”\’\<\,\>\.\?\/]{8,16}$");
        }
        /// <summary> 
        /// 验证标识符格式
        /// </summary>
        /// <param name="code">标识符（大小写字母、下划线、数字，1到20位）</param>
        /// <returns>验证结果</returns>
        public static bool IsCode(this string code)
        {
            return IsMatchRegEx(code, false, @"[A-Za-z0-9_]{1,20}");
        }
        /// <summary>
        /// 验证ip
        /// </summary>
        /// <param name="ip">ip</param>
        /// <returns></returns>
        public static bool IsIPAddress(this string ip)
        {
            if (ip == null || ip == string.Empty || ip.Length < 7 || ip.Length > 15) return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            return IsMatchRegEx(ip, false, regformat);
        }
        /// <summary>
        /// 验证身份证
        /// </summary>
        /// <param name="idNo">身份证号(包含15位和18位身份证号)</param>
        /// <returns></returns>
        public static bool IsIdNo(this string idNo)
        {
            string regformat = @"(^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$)|(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}(\d{1}|X)$)";
            return IsMatchRegEx(idNo, false, regformat);
        }
        /// <summary>
        /// 匹配正则
        /// </summary>
        /// <param name="text">要验证的文本</param>
        /// <param name="allowNull">是否允许空值</param>
        /// <param name="regExPattern">正则表达式</param>
        /// <returns>验证结果</returns>
        public static bool IsMatchRegEx(string text, bool allowNull, string regExPattern)
        {
            bool isEmpty = String.IsNullOrEmpty(text);
            if (isEmpty && allowNull) return true;
            if (isEmpty && !allowNull) return false;

            return Regex.IsMatch(text, regExPattern);
        }
    }
}
