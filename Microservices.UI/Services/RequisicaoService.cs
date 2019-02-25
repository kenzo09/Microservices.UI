using Microservices.UI.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Microservices.UI.Services
{
    public class RequisicaoService : IRequisicaoService
    {
        public async Task<HttpResponseMessage> PostAsync(dynamic data, Uri uri, string api)
        {
            var httpClient = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var baseUrl = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;

            return await httpClient.PostAsync($"{baseUrl}/api/{api}", content);
        }

        public async Task<HttpResponseMessage> GetAsync(Uri uri, string api, string parametros)
        {
            var httpClient = new HttpClient();
            var baseUrl = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;

            return await httpClient.GetAsync($"{baseUrl}/api/{api}{parametros}");
        }
    }
}
