using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Web.Areas.TrackApi.Models.Response
{
    public class ChartResponses
    {
        /// <summary>
        /// 图表名称
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// X轴时间数据
        /// </summary>
        public List<string> XAxisData { get; set; }

        /// <summary>
        /// 具体数据
        /// </summary>
        public List<SerieItemResponses> Series { get; set; }
    }

    public class SerieItemResponses
    {
        public string Name { get; set; }
        public List<decimal> Data { get; set; }
        public string Type { get; set; } = "line";
    }
}