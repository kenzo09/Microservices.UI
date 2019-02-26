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
    public class StoreMocController : Controller
    {
        [HttpGet]
        [Route("api/StoreMoc")]
        public IActionResult Index(StoreToGetMoc storeToGetMoc )
        {
            return Ok();
        }
    }
}