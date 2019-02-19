using System;
using System.Linq;
using System.Net;
using GeekBurger.Orders.Contract;
using GeekBurger_HTML.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace GeekBurger_HTML.Controllers
{
    public class MockController :Controller
    {        
        [HttpPost("api/face")]
        public IActionResult Face()
        {
            HttpContext.Request.Headers.TryGetValue("retries", out var stringValues);

            var tries = Convert.ToInt32(stringValues[0]);

            if (tries == 0 || tries % 3 != 0)
                return NotFound();
            else return Ok();
        }

        [HttpPost("/api/FoodRestrictions")]
        public IActionResult FoodRestrictions()
        {
            return Ok();
        }

        [HttpPost("/api/Order/Pay")]
        public IActionResult Pay()
        {
            return Ok();
        }

        [HttpPost("/api/Order")]
        public IActionResult Order([FromBody] OrderToPost order)
        {
            //I can't use UI Order now because it contains only one product
            return Json(new {OrderId = Guid.NewGuid() ,
                Total = string.Format("{0:C}", order.Products.Sum(x=> x.Price))});
        }
    }
}
