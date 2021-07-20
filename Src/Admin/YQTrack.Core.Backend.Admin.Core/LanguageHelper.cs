using System.Collections.Generic;
using System.Linq;
using YQTrack.Backend.LanguageHelper;

namespace YQTrack.Core.Backend.Admin.Core
{
    public static class LanguageHelper
    {
        /// <summary>
        /// 获取语言信息列表
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetLanguageList()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var items = LanguageManage.GetAllKey(LanguageType.GlobalLanguage);
            foreach (var item in items)
            {
                dynamic model = (dynamic)LanguageManage.GetJObject("zh-cn", LanguageType.GlobalLanguage, item);
                string code = (string)(model["_code"].Value).ToLower();
                dic.Add(code, model["_nameTran"].Value + "(" + code + ")");
            }
            return dic.OrderBy(o => o.Key).ToDictionary(key => key.Key, value => value.Value);
        }

        public static string GetCountryTextZhCn(string countryCode)
        {
            var text = LanguageManage.GetText("zh-cn", LanguageType.GlobalWDCountry, countryCode.PadLeft(4, '0'), "_name");
            return text;
        }

        /// <summary>
        /// 获取店铺平台类型列表
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetPlatformList()
        {
            return LanguageManage.GetJObjects("zh-cn", LanguageType.GlobalBisPlatform).ToDictionary(key => key.Value<int>("key"), value => value.Value<string>("_name"));
        }

        /// <summary>
        /// 获取店铺平台类型
        /// </summary>
        /// <returns></returns>
        public static string GetPlatformText(int key)
        {
            return LanguageManage.GetText("zh-cn", LanguageType.GlobalBisPlatform, key.ToString().PadLeft(2, '0'), "_name");
        }

        /// <summary>
        /// 获取大批量任务类型列表
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetBatchTaskTypeList()
        {
            return LanguageManage.GetJObjects("zh-cn", LanguageType.SellerETrackBatchTaskType).ToDictionary(key => key.Value<int>("key"), value => value.Value<string>("_name"));
        }

        /// <summary>
        /// 获取大批量任务类型
        /// </summary>
        /// <returns></returns>
        public static string GetBatchTaskTypeText(int key)
        {
            return LanguageManage.GetText("zh-cn", LanguageType.SellerETrackBatchTaskType, key.ToString(), "_name");
        }
    }
}
