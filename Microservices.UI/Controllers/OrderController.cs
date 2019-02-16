using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.UI.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.UI.Controllers
{
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : Controller
    {
        [HttpPost]
        [Route("api/Order")]
        public IActionResult Index(OrderRequest order)
        {
            return Ok(new OrderResponse());
        }
    }
}