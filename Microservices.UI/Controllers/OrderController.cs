using Microservices.UI.Contracts;
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