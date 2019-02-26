using Microservices.UI.Contracts;
using Microservices.UI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.UI.Controllers
{
    [Produces("application/json")]
    [Route("api/Order")]
    public class OrderController : Controller
    {
        private readonly INewOrderService _newOrderService;

        public OrderController(INewOrderService newOrderService)
        {
            _newOrderService = newOrderService;
        }

        [HttpPost]
        [Route("api/Order")]
        public IActionResult Index(OrderRequest order)
        {
            var orderResponse = new OrderResponse();

            _newOrderService.AddToMessageList("NewOrder", orderResponse);
            _newOrderService.SendMessagesAsync();

            return Ok(order);
        }
    }
}