using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using YQTrack.Backend.Enums;

namespace YQTrack.Core.Backend.Admin.Core
{
    public static class EnumExtension
    {
        public static string GetDescription(this System.Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo == null) return enumValue.ToString();
            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : enumValue.ToString();
        }

        public static string GetDisplayName(this System.Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo == null) return enumValue.ToString();
            var displayNameAttributes = (DisplayNameAttribute[])fieldInfo.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            if (displayNameAttributes.Any() && displayNameAttributes[0] != null)
            {
                return displayNameAttributes[0].DisplayName;
            }

            var displayAttributes = (DisplayAttribute[])fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
            return displayAttributes.Length > 0 ? displayAttributes[0].Name ?? displayAttributes[0].Description : enumValue.ToString();
        }

        public static int GetDefaultValue(this System.Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo == null) return 0;
            var defaultValueAttributes = (DefaultValueAttribute[])fieldInfo.GetCustomAttributes(typeof(DefaultValueAttribute), false);
            return defaultValueAttributes.Length > 0 ? Convert.ToInt32(defaultValueAttributes[0].Value) : 0;
        }

        public static bool ValidateImsIgnore(this System.Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo == null) return false;
            var imsIgnoreAttributes = (IMSIgnoreAttribute[])fieldInfo.GetCustomAttributes(typeof(IMSIgnoreAttribute), false);
            return imsIgnoreAttributes.Length <= 0;
        }
    }
}