using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.Deals.DTO.Output
{
    public class ChartOutput
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
        public List<SerieItemOutput> Series { get; set; }
    }

    public class SerieItemOutput
    {
        public string Name { get; set; }
        public List<decimal> Data { get; set; }
    }
}