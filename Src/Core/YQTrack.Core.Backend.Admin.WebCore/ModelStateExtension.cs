using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace YQTrack.Core.Backend.Admin.WebCore
{
    public static class ModelStateExtension
    {
        public static string GetAllErrorMsg(this ModelStateDictionary modelState)
        {
            return string.Join(";", modelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
        }
    }
}