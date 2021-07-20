using System;

namespace YQTrack.Core.Backend.Admin.Web.Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ExcelDateTimeFormatAttribute : Attribute
    {
        public string Format { get; }

        public ExcelDateTimeFormatAttribute(string format)
        {
            Format = format;
        }
    }
}