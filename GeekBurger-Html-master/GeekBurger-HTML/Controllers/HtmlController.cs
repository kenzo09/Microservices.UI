using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using GeekBurger_HTML.Models;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using GeekBurger_HTML.Services;
using Microsoft.Extensions.Configuration;
using GeekBurger_HTML.Configuration;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;

namespace GeekBurger_HTML.Controllers
{
    public class HtmlController : Controller
    {
        private string _baseUri;
        private readonly UiApiConfiguration _uIApiConfiguration;
        private readonly IHostingEnvironment _env;
        private readonly IDebugService _debugService;
        private readonly IReadOnlyPolicyRegistry<string> _policyRegistry;
        private readonly Guid RequesterId = Guid.NewGuid();
        private readonly ILogger _logger;
        public HtmlController(IHostingEnvironment env,
            IDebugService debugService,
            ILogger<HtmlController> logger,
            IReadOnlyPolicyRegistry<string> policyRegistry)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _uIApiConfiguration = config.GetSection("UIApi").Get<UiApiConfiguration>();

            _env = env;
            _debugService = debugService;
            _logger = logger;
            _policyRegistry = policyRegistry;
            _baseUri = _uIApiConfiguration.BaseUrl;
        }

        public IActionResult Index()
        {
            ViewBag.FoodRestrictionsApi = _uIApiConfiguration.FoodRestrictionsApi;
            ViewBag.OrderApi = _uIApiConfiguration.OrderApi;
            ViewBag.OrderPayApi = _uIApiConfiguration.OrderPayApi;
            ViewBag.RequesterId = RequesterId;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Face(string image)
        {
            var imageArray = image.Split(',');

            if (imageArray.Length <= 1)
                return BadRequest();

            var webrootPath = _env.WebRootPath;
            var path = Path.Combine(webrootPath, "face.png");
            var face = Convert.FromBase64String(imageArray[1]);

            using (var fs = new FileStream(path, FileMode.Create))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    var data = face;
                    bw.Write(data);
                    bw.Close();
                }
            }

            //submit to UI service
            var faceToPost = new FaceToPost() { Face = face, RequesterId = RequesterId };

            var response = PostToApi(faceToPost, _uIApiConfiguration.FaceApi).Result;

            return response.IsSuccessStatusCode ? Ok() : StatusCode(500);
        }

        private async Task<HttpResponseMessage> PostToApi(dynamic data, string apiUrl)
        {
            var client = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

            var content = new ByteArrayContent(byteData);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var retryPolicy = _policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>(PolicyNames.BasicRetry)
                              ?? Policy.NoOpAsync<HttpResponseMessage>();

            var context = new Context($"GetSomeData-{Guid.NewGuid()}", new Dictionary<string, object>
                {
                    { PolicyContextItems.Logger, _logger }, { "url", apiUrl }
                });

            var retries = 0;
            // ReSharper disable once AccessToDisposedClosure
            var response = await retryPolicy.ExecuteAsync((ctx) =>
            {
                client.DefaultRequestHeaders.Remove("retries");
                client.DefaultRequestHeaders.Add("retries", new[] { retries++.ToString() });

                var baseUrl = _baseUri;
                if (string.IsNullOrWhiteSpace(baseUrl))
                {
                    var uri = Request.GetUri();
                    baseUrl = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
                }

                var isValid = Uri.IsWellFormedUriString(apiUrl, UriKind.Absolute);

                return client.PostAsync(isValid ? $"{baseUrl}{apiUrl}" : $"{baseUrl}/api/Face", content);
            }, context);

            content.Dispose();

            return response;

        }

        [HttpGet]
        public IActionResult Debug(string command)
        {
            var message = command?.Split('|');
            if (message?.Length > 1)
                _debugService.SendMessageAsync(message[0], message[1], message.Length > 2 ? message[2] : null).Wait();

            return Json("OK");
        }
        [HttpPost]
        public IActionResult BaseUri(string baseUri)
        {
            _baseUri = baseUri;
            return Ok();
        }
    }

    public class LoggingEvents
    {
        public const int GenericFailure = 1000;
    }
}
