using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Log.DTO.Output;
using YQTrack.Core.Backend.Enums.Pay;
using YQTrack.Core.Backend.Enums.User;

namespace YQTrack.Core.Backend.Admin.Log.Service
{
    public interface IAnalysisService : IScopeService
    {
        Task<ChartOutput> GetAnalysisDataAsync(AnalysisType analysisType, ChartDateType chartDateType);
    }
}