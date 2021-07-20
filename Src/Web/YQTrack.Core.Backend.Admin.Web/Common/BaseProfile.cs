using AutoMapper;

namespace YQTrack.Core.Backend.Admin.Web.Common
{
    public abstract class BaseProfile : Profile
    {
        protected BaseProfile()
        {
            RecognizePrefixes("F");
            RecognizeDestinationPrefixes("F");
        }
    }
}