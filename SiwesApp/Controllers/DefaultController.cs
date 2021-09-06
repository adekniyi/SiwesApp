using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Controllers
{
    [ApiController]
    [Route("/")]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetDefault()
        {
            return Ok(new { programmer1 = "Adek Niyi" });
        }
    }
}
