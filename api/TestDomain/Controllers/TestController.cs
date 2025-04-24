using Microsoft.AspNetCore.Mvc;

namespace api.TestDomain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "hello world" });
        }
    }
}