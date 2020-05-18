using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace jwtproject.api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpPost("[action]")]
        public IActionResult Index()
        {
            List<string> model = new List<string>();
            model.Add("Enis");
            model.Add("Özlem");
            model.Add("Kağam");

            return Ok(JsonConvert.SerializeObject(model));
        }
    }
}