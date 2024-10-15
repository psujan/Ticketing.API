using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ticketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("all-categories")]
        public IActionResult Index()
        {
            return Ok("Controller Access Authorized");
        }
    }
}
