using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace YQTrack.Core.Backend.Admin.Core
{
    public static class JsonHelper
    {
        /// <summary>
        /// 将对象转换为Json字符串-字段名首字母小写
        /// </summary>
        /// <param name="target">目标对象</param>
        public static string ToJson(object target)
        {
            if (target == null)
                return "[]";
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                //Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Include,//是否忽略空（Null）对象输出
                ContractResolver = new CamelCasePropertyNamesContractResolver()
                //DefaultValueHandling = DefaultValueHandling.Ignore
            };
            string result = JsonConvert.SerializeObject(target, settings);
            return result;
        }
    }
}
