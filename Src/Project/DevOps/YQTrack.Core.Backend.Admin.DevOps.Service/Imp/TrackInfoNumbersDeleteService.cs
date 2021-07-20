using Newtonsoft.Json;
using System.Collections.Generic;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.DevOps.DTO;
using YQTrack.Log;

namespace YQTrack.Core.Backend.Admin.DevOps.Service.Imp
{
    public class TrackInfoNumbersDeleteService : ITrackInfoNumbersDeleteService
    {
        public TrackInfoNumbersDeleteService()
        {
        }


        /// <summary>
        /// 获取跟踪单号的完整缓存列表
        /// </summary>
        /// <param name="trackNos">跟踪单号</param>
        /// <returns></returns>
        public List<TrackCache> GetListTrackCache(string[] trackNos)
        {
            List<TrackCache> list = new List<TrackCache>();
            for (int i = 0; i < trackNos.Length; i++)
            {
                string tsKey = $"TS_{{{trackNos[i]}}}";
                IDictionary<string, string> tsDic = SentinelRedisHelper.Default.SentinelRedisControler2.HashGet(tsKey);
                if (tsDic.Count > 0)
                {
                    list.Add(new TrackCache { Key = trackNos[i], Id = i + 1, ParentId = -1 });
                    list.Add(new TrackCache { Key = tsKey, Id = i + trackNos.Length + 5, ParentId = i + 1 });
                    int zeroCarrier = -1, firstCarrier = -1, secondCarrier = -1;
                    foreach (var ts in tsDic)
                    {
                        TrackSummary tsDto = JsonConvert.DeserializeObject<TrackSummary>(ts.Value);

                        switch (tsDto.CarrierType)
                        {
                            case 0:
                                int.TryParse(ts.Key, out zeroCarrier);
                                break;
                            case 1:
                                int.TryParse(ts.Key, out firstCarrier);
                                break;
                            case 2:
                                int.TryParse(ts.Key, out secondCarrier);
                                break;
                            default:
                                break;
                        }
                    }
                    if (zeroCarrier > -1)
                    {
                        string zeroCarrierKey = $"TK0_{zeroCarrier}_{{{trackNos[i]}}}";
                        if (SentinelRedisHelper.Default.SentinelRedisControler0.KeyTTL(zeroCarrierKey) > -2)
                        {
                            list.Add(new TrackCache { Key = zeroCarrierKey, Id = i + trackNos.Length + 1, ParentId = i + 1 });
                        }
                    }
                    if (firstCarrier > -1)
                    {
                        string firstCarrierKey = $"TK1_{firstCarrier}_{{{trackNos[i]}}}";
                        if (SentinelRedisHelper.Default.SentinelRedisControler0.KeyTTL(firstCarrierKey) > -2)
                        {
                            list.Add(new TrackCache { Key = firstCarrierKey, Id = i + trackNos.Length + 2, ParentId = i + 1 });
                        }
                        string aotuSecondCarrierKey = $"TK2_{firstCarrier}_0_{{{trackNos[i]}}}";
                        if (SentinelRedisHelper.Default.SentinelRedisControler0.KeyTTL(aotuSecondCarrierKey) > -2)
                        {
                            list.Add(new TrackCache { Key = aotuSecondCarrierKey, Id = i + trackNos.Length + 3, ParentId = i + 1 });
                        }
                    }
                    if (secondCarrier > -1)
                    {
                        string secondCarrierKey = $"TK2_{firstCarrier}_{secondCarrier}_{{{trackNos[i]}}}";
                        if (SentinelRedisHelper.Default.SentinelRedisControler0.KeyTTL(secondCarrierKey) > -2)
                        {
                            list.Add(new TrackCache { Key = secondCarrierKey, Id = i + trackNos.Length + 4, ParentId = i + 1 });
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 删除跟踪单号缓存Key
        /// </summary>
        /// <param name="keys"></param>
        public void DeleteKeys(string[] keys)
        {
            foreach (var key in keys)
            {
                if (key.StartsWith("TS"))
                {
                    if (!SentinelRedisHelper.Default.SentinelRedisControler2.KeyDelete(key))
                    {
                        LogHelper.LogObj(new LogDefinition(LogLevel.Error, "TrackCacheError"), new { msg = "缓存删除失败", key = key });
                    }
                }
                else
                {
                    if (!SentinelRedisHelper.Default.SentinelRedisControler0.KeyDelete(key))
                    {
                        LogHelper.LogObj(new LogDefinition(LogLevel.Error, "TrackCacheError"), new { msg = "缓存删除失败", key = key });
                    }
                }
            }
        }

    }

}
