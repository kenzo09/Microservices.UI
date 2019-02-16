using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.UI.Moc.Contratos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.UI.Moc.Controllers
{
    [Produces("application/json")]
    
    public class ProductsMocController : Controller
    {
        [HttpPost]
        [Route("api/ProductsMoc")]
        public IActionResult Index(FoodRestrictionsMoc foodRestrictions)
        {
            return Ok(new UserFoodRestrictionsMoc());
        }
    }
}