using System;
using System.ComponentModel.DataAnnotations;

namespace YQTrack.Core.Backend.Admin.WebCore
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class NotEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult($"参数{validationContext.DisplayName}不能为空", new[] { validationContext.MemberName });
            }

            if (value is int num)
            {
                if (num <= 0)
                {
                    return new ValidationResult($"参数{validationContext.DisplayName}不能小于等于0", new[] { validationContext.MemberName });
                }
            }

            if (value is long longNum)
            {
                if (longNum <= 0)
                {
                    return new ValidationResult($"参数{validationContext.DisplayName}不能小于等于0", new[] { validationContext.MemberName });
                }
            }

            return ValidationResult.Success;
        }
    }
}