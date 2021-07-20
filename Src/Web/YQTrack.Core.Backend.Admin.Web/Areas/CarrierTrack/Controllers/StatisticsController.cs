using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Input;
using YQTrack.Core.Backend.Admin.CarrierTrack.DTO.Output;
using YQTrack.Core.Backend.Admin.CarrierTrack.Service;
using YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Request;
using YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Models.Response;
using YQTrack.Core.Backend.Admin.Web.Common;
using YQTrack.Core.Backend.Admin.WebCore;
// ReSharper disable PossibleInvalidOperationException

namespace YQTrack.Core.Backend.Admin.Web.Areas.CarrierTrack.Controllers
{
    public class StatisticsController : BaseCarrierTrackController
    {
        private readonly IStatisticsService _statisticsService;
        private readonly IMapper _mapper;

        public StatisticsController(IStatisticsService statisticsService,
            IMapper mapper)
        {
            _statisticsService = statisticsService;
            _mapper = mapper;
        }

        /// <summary>
        /// 跟踪统计分析数据图标视图界面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取跟踪统计分析数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ModelStateValidationFilter]
        [PermissionCode(nameof(Index))]
        public async Task<IActionResult> TrackAnalysis(ChartRequest request)
        {
            // ReSharper disable once PossibleInvalidOperationException
            var output = await _statisticsService.GetTrackAnalysisOutputAsync(request.ChartDateType.Value, request.Email, request.Enable);
            var responses = _mapper.Map<ChartResponses>(output);
            return MyJson(responses);
        }

        /// <summary>
        /// 用户标签管理功能使用情况统计
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult UserMarkLog()
        {
            return View();
        }

        /// <summary>
        /// 获取用户标签管理功能使用情况统计数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [PermissionCode(nameof(UserMarkLog))]
        public async Task<IActionResult> GetUserMarkLogPageData(UserMarkLogPageDataRequest request)
        {
            var input = _mapper.Map<UserMarkLogPageDataInput>(request);
            var (outputs, total) = await _statisticsService.GetUserMarkLogPageDataAsync(input);
            var responses = _mapper.Map<IEnumerable<UserMarkLogPageDataResponse>>(outputs);
            return new JsonResult(new PageResponse<UserMarkLogPageDataResponse>
            {
                Data = responses,
                Count = total
            });
        }

        [HttpGet]
        [FileDownload]
        [ModelStateValidationFilter]
        public async Task<IActionResult> Export([FromQuery]ExportRequest request)
        {
            var outputs = await _statisticsService.GetExportAsync(request.StartTime.Value, request.EndTime.Value);
            var bytes = GenerateExcel(outputs);
            return FileExcel(bytes);
        }

        /// <summary>
        /// 生成报表Excel数据
        /// </summary>
        /// <param name="outputs"></param>
        /// <returns></returns>
        private static byte[] GenerateExcel(IEnumerable<ReportOutput> outputs)
        {
            using (var excel = new ExcelPackage())
            {
                var sheet = excel.Workbook.Worksheets.Add("按账号统计每日导入数据量");

                // 生成sheet左上角
                var cellRang = sheet.Cells[1, 1];
                cellRang.Value = "账号";
                cellRang.Style.Font.Bold = true;
                cellRang.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                cellRang.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                // 生成日期列
                var dateList = outputs.OrderBy(x => x.Date).Select(x => x.Date.ToString("yyyy/MM/dd")).ToList();
                for (var i = 0; i < dateList.Count; i++)
                {
                    var excelRange = sheet.Cells[1, i + 2];
                    excelRange.Value = dateList[i];
                    excelRange.Style.Font.Bold = true;
                    excelRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    excelRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                }

                // 生成账号列
                var emailList = outputs.Select(x => x.UserImportOutputs.Select(c => c.Email)).SelectMany(x => x).Distinct().OrderBy(x => x).ToList();
                for (var i = 0; i < emailList.Count; i++)
                {
                    var excelRange = sheet.Cells[i + 2, 1];
                    excelRange.Value = emailList[i];
                    excelRange.Style.Font.Bold = false;
                    excelRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    excelRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                }

                // 生成具体数据单元格
                for (var i = 0; i < dateList.Count; i++)
                {
                    var date = dateList[i];
                    var userImportOutputs = outputs.Single(x => x.Date.ToString("yyyy/MM/dd") == date).UserImportOutputs;
                    for (var j = 0; j < emailList.Count; j++)
                    {
                        var email = emailList[j];
                        var excelRange = sheet.Cells[j + 2, i + 2];
                        var userImportOutput = userImportOutputs.SingleOrDefault(x => x.Email == email);
                        excelRange.Value = userImportOutput?.Import ?? 0;
                        excelRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        excelRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    }
                }

                sheet.Cells.AutoFitColumns();

                return excel.GetAsByteArray();
            }
        }

        [HttpGet]
        [FileDownload]
        [ModelStateValidationFilter]
        public async Task<IActionResult> ExportUserMarkLog([FromQuery]ExportUserMarkLogRequest request)
        {
            var outputs = await _statisticsService.GetExportUserMarkLogAsync(request.Email, request.StartTime, request.EndTime);
            var responses = _mapper.Map<IEnumerable<ExportUserMarkLogResponse>>(outputs);
            var bytes = ExcelHelper.GenerateExcel(responses);
            return FileExcel(bytes);
        }
    }
}