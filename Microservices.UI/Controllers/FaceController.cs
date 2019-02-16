using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microservices.UI.Contracts;
using Microservices.UI.Moc.Contratos;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Microservices.UI.Controllers
{
    [Produces("application/json")]   
    public class FaceController : Controller
    {
        [HttpPost]
        [Route("api/Face")]
        public IActionResult Index(FaceToPost face)
        {
            var x = new Xpto(); x.DoSomething(Request.GetUri(), "User");

            return Ok(new FaceToProcessing());
        }
    }


    public class Xpto
    {
        public async void DoSomething(Uri uri, string api)
        {
            var httpClient = new HttpClient();
            var byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new UserToPost()));
            var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var baseUrl = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;

            var response = await httpClient.PostAsync($"{baseUrl}/api/{api}", content);


        }
    }
}