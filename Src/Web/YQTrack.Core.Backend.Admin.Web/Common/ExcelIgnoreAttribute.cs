using System;

namespace YQTrack.Core.Backend.Admin.Web.Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public class ExcelIgnoreAttribute : Attribute
    {
    }
}