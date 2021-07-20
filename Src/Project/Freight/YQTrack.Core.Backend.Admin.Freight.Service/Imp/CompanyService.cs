using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Freight.Data;
using YQTrack.Core.Backend.Admin.Freight.Data.Models;
using YQTrack.Core.Backend.Admin.Freight.DTO.Input;
using YQTrack.Core.Backend.Admin.Freight.DTO.Output;
using YQTrack.Core.Backend.Admin.Freight.Service.RemoteApi;
using YQTrack.Core.Backend.Admin.User.Data;
using YQTrack.Core.Backend.Enums.Freight;

namespace YQTrack.Core.Backend.Admin.Freight.Service.Imp
{
    public class CompanyService : ICompanyService
    {
        private readonly CarrierContext _dbContext;
        private readonly UserDbContext _userDbContext;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptionsSnapshot<FreightConfig> _freightOptions;

        public CompanyService(CarrierContext dbContext,
                              UserDbContext userDbContext,
                              IMapper mapper,
                              IHttpClientFactory httpClientFactory,
                              IOptionsSnapshot<FreightConfig> freightOptions)
        {
            _dbContext = dbContext;
            _userDbContext = userDbContext;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _freightOptions = freightOptions;
        }

        public async Task<(IEnumerable<CompanyPageDataOutput> outputs, int total)> GetCompanyPageDataAsync(CompanyPageDataInput input)
        {
            long userId = 0;
            if (!string.IsNullOrWhiteSpace(input.Email))
            {
                var user = await _userDbContext.TuserInfo.FirstOrDefaultAsync(x => x.Femail == input.Email.Trim());
                if (user != null)
                    userId = user.FuserId;
            }

            var queryable = _dbContext.Tcompany
                .WhereIf(() => !string.IsNullOrWhiteSpace(input.CompanyName), x => x.FcompanyName.Contains(input.CompanyName.Trim()))
                .WhereIf(() => input.Status.HasValue, x => x.FcheckState == (byte)input.Status.Value)
                .WhereIf(() => !string.IsNullOrWhiteSpace(input.Email) || userId > 0, x => x.Femail.Contains(input.Email) || x.FuserId == userId);

            var count = await queryable.CountAsync();

            var outputs = await queryable.OrderBy(x => x.FcheckState)
                .ThenByDescending(x => x.FcreateTime)
                .ProjectTo<CompanyPageDataOutput>()
                .ToPage(input.Page, input.Limit)
                .ToListAsync();

            return (outputs, count);
        }

        private async Task<Tcompany> GetCompanyByIdAsync(long id)
        {
            var company = await _dbContext.Tcompany.SingleOrDefaultAsync(x => x.FcompanyId == id);
            if (company == null)
            {
                throw new BusinessException($"参数错误:{nameof(id)}:{id},找不到该运输商公司");
            }
            return company;
        }

        private async Task<TcompanyConfig> GetCompanyConfigByIdAsync(long id)
        {
            var config = await _dbContext.TcompanyConfig.SingleOrDefaultAsync(x => x.FcompanyId == id);
            if (config == null)
            {
                throw new BusinessException($"参数错误:{nameof(id)}:{id},找不到公司配置");
            }
            return config;
        }

        public async Task<CompanyEditOutput> GetCompanyEditInfoAsync(long id)
        {
            var company = await GetCompanyByIdAsync(id);
            var config = await GetCompanyConfigByIdAsync(id);
            var result = new CompanyEditOutput
            {
                Code = company.Fcode,
                Limit = config.FmaxChannel,
                Phone = company.Fmobile,
                Url = company.Furl
            };
            return result;
        }

        public async Task EditAsync(CompanyEditInput input)
        {
            var company = await GetCompanyByIdAsync(input.Id);
            var config = await GetCompanyConfigByIdAsync(input.Id);
            company.Fcode = input.Code;
            company.Furl = input.Url;
            company.Fmobile = input.Phone;
            company.FupdateTime = DateTime.UtcNow;
            config.FupdateTime = DateTime.UtcNow;
            config.FmaxChannel = input.Limit;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CompanyViewCheckOutput> GetCompanyViewCheckInfoAsync(long id)
        {
            var company = await GetCompanyByIdAsync(id);
            var output = _mapper.Map<CompanyViewCheckOutput>(company);

            // 特别说明 --> 直接拼接完整请求地址使用(废弃:发布外网正式访问不了内网资源 修正:使用内部资源中转的方式)
            //output.Flogo = $"{_freightOptions.Value.CompanyAdminRemoteUrl}/{output.Flogo}?v={DateTime.UtcNow.Ticks.ToString()}";
            //output.Fimg = $"{_freightOptions.Value.CompanyAdminRemoteUrl}/{output.Fimg}?v={DateTime.UtcNow.Ticks.ToString()}";
            return output;
        }

        /// <summary>
        /// 使用内部请求局域网资源以提供外网访问
        /// </summary>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public async Task<byte[]> GetViewCheckImageAsync(string imgUrl)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var bytes = await httpClient.GetByteArrayAsync(new Uri(new Uri(_freightOptions.Value.CompanyAdminRemoteUrl), $"{imgUrl}?v={DateTime.UtcNow.Ticks.ToString()}"));
            return bytes;
        }

        public async Task PassAsync(long id)
        {
            var company = await GetCompanyByIdAsync(id);
            company.FcheckState = (int)CompanyAuditState.AuditPass;
            company.FcheckTime = DateTime.UtcNow;
            company.FupdateTime = DateTime.UtcNow;

            await CallRemoteApiAsync(new Dictionary<string, string>
            {
                {"userId",company.FuserId.ToString() },
                {"success",true.ToString() }
            });

            await _dbContext.SaveChangesAsync();
        }

        private async Task CallRemoteApiAsync(Dictionary<string, string> param)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(new Uri(_freightOptions.Value.CompanyAdminRemoteUrl), "/Login/SendEmailToCarrierForRegister"))
            {
                Content = new FormUrlEncodedContent(param)
            };
            request.Headers.Add("AuthKey", "ims");
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new BusinessException($"调用远程API失败,数据库修改失败,请重试,{response.ReasonPhrase}");
            }
            var str = await response.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<ApiResult>(str);
            if (!apiResult.Success)
            {
                throw new BusinessException(apiResult.Msg);
            }
        }

        public async Task RejectAsync(long id, string desc)
        {
            var company = await GetCompanyByIdAsync(id);
            var combineHistory = string.IsNullOrWhiteSpace(company.FcheckDescHistory) ? $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}:{desc}" : $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}:{desc} </br>{company.FcheckDescHistory}";
            company.FcheckState = (int)CompanyAuditState.AuditFail;
            company.FcheckDesc = desc;
            company.FcheckTime = DateTime.UtcNow;
            company.FupdateTime = DateTime.UtcNow;
            company.FcheckDescHistory = combineHistory;
            await CallRemoteApiAsync(new Dictionary<string, string>
            {
                {"userId",company.FuserId.ToString() },
                {"success",false.ToString() },
                {"reason",desc }
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}