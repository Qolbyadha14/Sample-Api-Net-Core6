using api.prostasia.id.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace OrderServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
        }

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Get()
        {
            
            var id = Environment.MachineName;
            var pid = System.Diagnostics.Process.GetCurrentProcess().Id;
            return Ok("Api Services Runnning," + id + "," + pid +",");
        }

    }
}
