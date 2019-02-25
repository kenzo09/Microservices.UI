using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microservices.UI.Contracts;
using Microservices.UI.Moc.Contratos;
using Microservices.UI.Services;
using Microservices.UI.Services.Interfaces;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Microservices.UI.Controllers
{
    [Produces("application/json")]   
    public class FaceController : Controller
    {
        private readonly IRequisicaoService _requisicaoService;

        public FaceController(IRequisicaoService requisicao)
        {
            _requisicaoService = requisicao;
        }

        [HttpPost]
        [Route("api/Face")]
        public IActionResult Index(FaceToPost face)
        {
            var response = _requisicaoService.PostAsync(new UserToPostMoc(), Request.GetUri(), "UserMoc");

            return Ok(new FaceToProcessing());
        }
    }
}