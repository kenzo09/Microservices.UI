using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microservices.UI.Moc.Contratos;


namespace Microservices.UI.Moc.Controllers
{
    [Produces("application/json")]
    [Route("api/UserFoodRestrictionsMoc")]
    public class UserFoodRestrictionsMocController : Controller
    {
        [HttpPost]
        [Route("api/User")]
        public IActionResult Index(FoodRestrictions foodRestrictions)
        {
            return Ok(new UserFoodRestrictions());
        }
    }
}