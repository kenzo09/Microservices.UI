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
    public class UserController : Controller
    {
        [HttpPost]
        [Route("api/User")]
        public IActionResult Index(UserToPost userToPost)
        {
            return Ok(new UserToResponse());
        }



    }
}