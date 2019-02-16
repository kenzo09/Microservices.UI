using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microservices.UI.Moc.Contratos;
using Microservices.UI.Contracts;

namespace Microservices.UI.Moc.Controllers
{
    [Produces("application/json")]    
    public class UserFoodRestrictionsMocController : Controller
    {
        [HttpPost]
        [Route("api/Users/foodRestricions")]
        public IActionResult Index(FoodRestrictions foodRestrictions)
        {
            return Ok(new UserFoodRestrictionsMoc());
        }
    }
}