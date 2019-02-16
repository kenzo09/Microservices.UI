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
    public class FaceController : Controller
    {
        [HttpPost]
        [Route("api/Face")]
        public IActionResult Index(FaceToPost face)
        {
            return Ok(new FaceToProcessing());
        }
    }
}