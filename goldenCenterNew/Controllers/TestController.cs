using Microsoft.AspNetCore.Mvc;

namespace goldenCenterNew.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("hello")]
        public IActionResult GetHello()
        {
            return Ok(new { message = "API is working!" });
        }
    }
}
