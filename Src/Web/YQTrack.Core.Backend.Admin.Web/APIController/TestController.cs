using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using YQTrack.Core.Backend.Admin.Freight.Data;
using YQTrack.Core.Backend.Admin.Freight.Data.Models;
using YQTrack.Core.Backend.Admin.User.Data;

namespace YQTrack.Core.Backend.Admin.Web.APIController
{
    /// <summary>
    /// 测试API控制器
    /// </summary>
    [AllowAnonymous]
    public class TestController : BaseApiController
    {
        private readonly CarrierContext _dbContext;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly UserDbContext _userDbContext;

        public TestController(CarrierContext dbContext, IHostingEnvironment hostingEnvironment,
                              UserDbContext userDbContext)
        {
            _dbContext = dbContext;
            _hostingEnvironment = hostingEnvironment;
            _userDbContext = userDbContext;
        }

        /// <summary>
        /// 获取10条渠道信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("getchannels")]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Tchannel>>> GetTodoItems()
        {
            return await _dbContext.Tchannel.OrderByDescending(x => x.FcreateTime).Take(5).ToListAsync();
        }

        /// <summary>
        /// 获取5条询价单状态日志
        /// </summary>
        /// <returns></returns>
        [HttpGet("getlogs")]
        [ProducesResponseType(200)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<TInquiryOrderStatusLog>>> GetSomeLogs()
        {
            return await _dbContext.TInquiryOrderStatusLog.OrderByDescending(x => x.FCreateTime).Take(5).ToListAsync();
        }

        /// <summary>
        /// 导出示例Excel文件
        /// </summary>
        /// <returns></returns>
        [HttpGet("export")]
        [ProducesResponseType(200)]
        public IActionResult Export()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = $"{Guid.NewGuid()}.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // 添加worksheet
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("aspnetcore");
                //添加头
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Url";
                //添加值
                worksheet.Cells["A2"].Value = 1000;
                worksheet.Cells["B2"].Value = "LineZero";
                worksheet.Cells["C2"].Value = "http://www.cnblogs.com/linezero/";

                worksheet.Cells["A3"].Value = 1001;
                worksheet.Cells["B3"].Value = "LineZero GitHub";
                worksheet.Cells["C3"].Value = "https://github.com/linezero";
                worksheet.Cells["C3"].Style.Font.Bold = true;

                package.Save();
            }
            return File(sFileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        /// <summary>
        /// 获取当前17用户数量
        /// </summary>
        /// <returns></returns>
        [HttpGet("getUserCount")]
        public async Task<ActionResult<int>> GetUserCount()
        {
            var count = await (from tuserInfo in _userDbContext.TuserInfo
                               join tuserProfile in _userDbContext.TuserProfile on tuserInfo.FuserId equals tuserProfile.FuserId
                               select tuserInfo.FuserId).CountAsync();
            return count;
        }
    }
}