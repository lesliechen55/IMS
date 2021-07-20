using System;
using System.Collections.Generic;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Core
{
    public static class EnumHelper
    {
        public static Dictionary<int, string> GetSelectItem<T>(bool isDisplay = false, bool isIgnore = true) where T : System.Enum
        {
            var dict = new Dictionary<int, string>();
            foreach (var item in System.Enum.GetValues(typeof(T)))
            {
                var itemValue = (T)item;
                if (isIgnore)
                {
                    var fieldInfo = itemValue.GetType().GetField(itemValue.ToString());
                    if (fieldInfo == null) continue;
                    var ignoreAttributes = (IMSIgnoreAttribute[])fieldInfo.GetCustomAttributes(typeof(IMSIgnoreAttribute), false);
                    if (ignoreAttributes.Length == 0)
                    {
                        dict.Add(Convert.ToInt32(item), isDisplay ? itemValue.GetDisplayName() : itemValue.GetDescription());
                    }
                }
                else
                {
                    dict.Add(Convert.ToInt32(item), isDisplay ? itemValue.GetDisplayName() : itemValue.GetDescription());
                }
            }
            return dict;
        }

        public static IEnumerable<string> GetDescriptionList<T>(bool isDisplay = false) where T : System.Enum
        {
            var list = new List<string>();
            foreach (var item in System.Enum.GetValues(typeof(T)))
            {
                list.Add(isDisplay ? ((T)item).GetDisplayName() : ((T)item).GetDescription());
            }
            return list;
        }
    }
}
