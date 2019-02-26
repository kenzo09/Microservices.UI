using Microservices.UI.Contracts;
using Microservices.UI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Microservices.UI.Controllers
{
    [Produces("application/json")]    
    public class FoodRestrictionsController : Controller
    {
        private readonly IRequisicaoService _requisicaoService;
        private readonly IConfigurationService _configurationService;

        public FoodRestrictionsController(IRequisicaoService requisicao, IConfigurationService configurationService)
        {
            _configurationService = configurationService;
            _requisicaoService = requisicao;
        }

        [HttpPost]
        [Route("api/FoodRestrictions")]
        public IActionResult Index(FoodRestrictions foodRestrictions)
        {
            var url = _configurationService.GetConfigValue(typeof(string), "UsersUri").ToString();
            Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri uri);
            bool usarMoc = Convert.ToBoolean(_configurationService.GetConfigValue(typeof(bool), "UsarMoc"));

            string api = usarMoc ? "Users/foodRestricionsMoc" : "Users/foodRestricions";
            var response = _requisicaoService.PostAsync(foodRestrictions, uri, api);

            return Ok();
        }
    }
}