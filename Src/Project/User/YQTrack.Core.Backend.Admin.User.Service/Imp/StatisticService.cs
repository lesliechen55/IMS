using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.User.DTO.Output;

namespace YQTrack.Core.Backend.Admin.User.Service.Imp
{
    public class StatisticService : IStatisticService
    {
        public async Task<IEnumerable<OrderByDayOutput>> GetOrderByDayDataAsync(int stateType, int dayType)
        {
            // 表示查询今天的日订单量
            if (dayType == 1)
            {

            }
            else
            {

            }

            var outputs = new List<OrderByDayOutput>
            {
                new OrderByDayOutput
                {
                    Date="2019-05-01",
                    Count = 78
                },
                new OrderByDayOutput
                {
                    Date="2019-05-02",
                    Count = 100
                },
                new OrderByDayOutput
                {
                    Date="2019-05-03",
                    Count = 66
                },
                new OrderByDayOutput
                {
                    Date="2019-05-04",
                    Count = 77
                },
                new OrderByDayOutput
                {
                    Date="2019-05-05",
                    Count = 100
                },
                new OrderByDayOutput
                {
                    Date="2019-05-06",
                    Count = 88
                },
                new OrderByDayOutput
                {
                    Date="2019-05-07",
                    Count = 111
                }
            };

            await Task.Delay(TimeSpan.FromSeconds(2));

            return outputs;
        }
    }
}