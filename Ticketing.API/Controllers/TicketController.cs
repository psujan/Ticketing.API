using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.API.Model.Dto;

namespace Ticketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        [HttpPost]
        [Authorize]
        public async  Task<IActionResult> Create([FromBody]TicketRequestDto ticketRequest)
        {
            return Ok("Here");
        }
    }
}
