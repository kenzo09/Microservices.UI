using Microservices.UI.Contracts;
using Microservices.UI.Moc.Contratos;
using Microservices.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Microservices.UI.Controllers
{
    [Produces("application/json")]   
    public class FaceController : Controller
    {
        private readonly IRequisicaoService _requisicaoService;
        private readonly IConfigurationService _configurationService;

        public FaceController(IRequisicaoService requisicao, IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _requisicaoService = requisicao;
        }

        [HttpPost]
        [Route("api/Face")]
        public IActionResult Index(FaceToPost face)
        {
            var url = _configurationService.GetConfigValue(typeof(string), "UsersUri").ToString();
            Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri);
            bool usarMoc = Convert.ToBoolean(_configurationService.GetConfigValue(typeof(bool), "UsarMoc"));

            string api = usarMoc ? "UserMoc" : "User";
            var response = _requisicaoService.PostAsync(new UserToPostMoc(), uri, api);

            return Ok(new FaceToProcessing());
        }
    }
}