using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YQTrack.Core.Backend.Admin.Core;
using YQTrack.Core.Backend.Admin.Message.DTO;
using YQTrack.Standard.Common.Utils;

namespace YQTrack.Core.Backend.Admin.Message.Service.RemoteApi
{
    public class MessageService : IScopeService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MessageConfig _urlConfig;

        public MessageService(IHttpClientFactory httpClientFactory, IOptions<MessageConfig> urlConfigOptions)
        {
            _httpClientFactory = httpClientFactory;
            _urlConfig = urlConfigOptions.Value;
        }
        /// <summary>
        /// 渲染模板内容
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<string> RenderAsync(TemplateRenderData data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.TemplateContent))
            {
                return "";
            }
            ////样例数据为空，不需要渲染，直接返回原数据
            //if (string.IsNullOrWhiteSpace(data.DataJson))
            //{
            //    return data.TemplateContent;
            //}
            MessageRequestBase<TemplateRenderData> requestData = new MessageRequestBase<TemplateRenderData>
            {
                Method = "GetTemplatePreview",
                Version = "1.2",
                SourceType = 0,
                Param = data
            };

            var request = new HttpRequestMessage(HttpMethod.Post, _urlConfig.RemoteUrl)
            {
                Content = new StringContent(JsonHelper.ToJson(requestData), Encoding.UTF8)
            };
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //request.Headers.Add("AuthKey", "ims");
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new BusinessException("调用远程API失败，请重试");
            }

            string res = await response.Content.ReadAsStringAsync();
            ResultBase result = SerializeExtend.MyDeserializeFromJson<ResultBase>(res);

            if (string.IsNullOrWhiteSpace(result.Json))
            {
                throw new BusinessException("模板渲染失败，请检查模板内容");
            }
            return result.Json;
        }
    }
}
