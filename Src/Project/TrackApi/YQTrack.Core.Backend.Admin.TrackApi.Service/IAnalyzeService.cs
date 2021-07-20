using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.TrackApi.DTO.Output;
using YQTrack.Core.Backend.Enums.Pay;

namespace YQTrack.Core.Backend.Admin.TrackApi.Service
{
    public interface IAnalyzeService : IScopeService
    {
        Task<ChartOutput> GetRegisterAnalysisOutputAsync(ChartDateType chartDateType, int? userNo, string userName);

        Task<IEnumerable<AutoCompleteOutput>> GetAutoCompleteOutputAsync(string userName);
    }
}