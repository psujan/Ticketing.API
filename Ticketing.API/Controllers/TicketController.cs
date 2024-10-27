using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ticketing.API.Model.Domain;
using Ticketing.API.Model;
using Ticketing.API.Model.Dto;
using Ticketing.API.Repositories;
using Ticketing.API.Repositories.Interfaces;
using System.Net;
using Ticketing.API.Validations;

namespace Ticketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

       

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var data = await ticketRepository.GetById(id);
            return Ok(new ApiResponse<Ticket?>()
            {
                Success = true,
                Message = "Data Fetched Successfully",
                Data = data
            });
        }

        [HttpPost]
        [Authorize]
        [ValidateModel]
        [Consumes("multipart/form-data")]
        public async  Task<IActionResult> Create([FromForm]TicketRequestDto ticketRequest)
        {
            try
            {
                var ticket = await ticketRepository.Create(ticketRequest);
                return Ok(new ApiResponse<Ticket>()
                {
                    Success = true,
                    Message = "Ticket Created Successfully",
                    Data = ticket
                });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new ApiResponse<string>()
                {
                    Success = false,
                    Message = ex.Message,
                    Data = ""
                })
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }

        }
    }
}
