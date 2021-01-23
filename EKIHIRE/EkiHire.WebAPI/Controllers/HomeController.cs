using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EkiHire.WebAPI.Controllers
{
    public class HomeController : BaseController
    {

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("EkiHire.Web Api is running");
        }
    }
}
