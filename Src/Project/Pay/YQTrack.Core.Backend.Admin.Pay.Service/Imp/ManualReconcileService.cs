using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Data;
using YQTrack.Core.Backend.Admin.Data.Entity;
using YQTrack.Core.Backend.Admin.Pay.DTO.Input;
using YQTrack.Core.Backend.Admin.Pay.DTO.Output;

namespace YQTrack.Core.Backend.Admin.Pay.Service.Imp
{
    public class ManualReconcileService : IManualReconcileService
    {
        private readonly AdminDbContext _adminDbContext;

        public ManualReconcileService(AdminDbContext adminDbContext)
        {
            _adminDbContext = adminDbContext;
        }

        public async Task<bool> ExistAsync(string md5)
        {
            return await _adminDbContext.ManualReconcile.AnyAsync(x => x.FFileMd5 == md5.ToUpper());
        }

        public async Task ImportGlocashAsync(string json, string fileName, string md5, string storagePath, int year, int month, string remark, int operatorId)
        {
            #region 切割小文件逻辑

            IEnumerable<IEnumerable<string>> jsonArrayArray;
            try
            {
                jsonArrayArray = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<string>>>(json);
            }
            catch (Exception exception)
            {
                throw new BusinessException($"反序列化异常,Json文件格式错误", exception);
            }
            if (jsonArrayArray == null || !jsonArrayArray.Any())
            {
                throw new BusinessException($"Json内容为空或者反序列化无结果");
            }
            var orders = jsonArrayArray.Skip(1).ToList();
            if (!orders.Any())
            {
                throw new BusinessException($"{fileName}不包含任何有效对账订单信息");
            }

            // 按照订单中的日志切割文件分布存储且发现当然日期有文件则覆盖,否正生成一个空的日期文件
            var dateOrderList = orders.GroupBy(x => x.Last().ToString().Substring(0, 10)).OrderBy(x => x.Key).ToList();

            // 判断文件中的订单日期是否在选择的月份中
            var any = dateOrderList.Select(x => x.Key).Any(x =>
              {
                  var dateTime = DateTime.Parse(x);
                  if (dateTime.Year != year || dateTime.Month != month) return true;
                  return false;
              });
            if (any)
            {
                throw new BusinessException($"{year}-{month.ToString().PadLeft(2, '0')}与Json内容的订单日期不匹配,必须所有订单日期在所选择的年月范围内,请检查是否选择日期错误");
            }

            var days = DateTime.DaysInMonth(year, month);

            var header = jsonArrayArray.First().ToList();

            for (var date = 1; date <= days; date++)
            {
                var dateStr = $"{year}-{month.ToString().PadLeft(2, '0')}-{date.ToString().PadLeft(2, '0')}";
                var orderList = dateOrderList.FirstOrDefault(x => x.Key == dateStr);
                if (orderList != null)
                {
                    var list = orderList.ToList();
                    list.Insert(0, header);
                    var dateOrderListJson = JsonConvert.SerializeObject(list, Formatting.Indented);
                    await File.WriteAllTextAsync($"{storagePath}/{dateStr}.json", dateOrderListJson, Encoding.UTF8);
                }
                else
                {
                    await File.WriteAllTextAsync($"{storagePath}/{dateStr}.json", "[[]]", Encoding.UTF8);
                }
            }


            #endregion

            var manualReconcile = new ManualReconcile
            {
                FCreatedBy = operatorId,
                FFileMd5 = md5.ToUpper(),
                FFileName = fileName,
                FFilePath = Path.Combine(storagePath, fileName),
                FMonth = month,
                FYear = year,
                FOrderCount = orders.Count,
                FRemark = remark
            };
            await _adminDbContext.ManualReconcile.AddAsync(manualReconcile);
            await _adminDbContext.SaveChangesAsync();
        }

        public async Task<(IEnumerable<ManualReconcilePageDataOutput> outputs, int total)> GetPageDataAsync(ManualReconcilePageDataInput input)
        {
            var queryable = _adminDbContext.ManualReconcile.WhereIf(() => input.Year.HasValue, x => x.FYear == input.Year.Value)
                .WhereIf(() => input.Month.HasValue, x => x.FMonth == input.Month.Value);
            var total = await queryable.CountAsync();
            var outputs = await queryable
                .OrderByDescending(x => x.FCreatedTime)
                .ToPage(input.Page, input.Limit).ProjectTo<ManualReconcilePageDataOutput>().ToListAsync();
            return (outputs, total);
        }
    }
}