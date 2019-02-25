using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.UI.Contracts;
using Microservices.UI.Moc.Contratos;
using Microservices.UI.Services;
using Microservices.UI.Services.Interfaces;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.UI.Controllers
{
    [Produces("application/json")]    
    public class FoodRestrictionsController : Controller
    {
        private readonly IRequisicaoService _requisicaoService;

        public FoodRestrictionsController(IRequisicaoService requisicao)
        {
            _requisicaoService = requisicao;
        }

        [Route("api/FoodRestrictions")]
        public IActionResult Index(FoodRestrictions foodRestrictions)
        {

            var response = _requisicaoService.PostAsync(foodRestrictions, Request.GetUri(), "Users/foodRestricionsMoc");

            return Ok();
        }
    }
}