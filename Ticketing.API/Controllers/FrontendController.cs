using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Ticketing.API.Model.Dto.Requuest;
using Ticketing.API.Model.Dto;
using Ticketing.API.Model;
using Ticketing.API.Repositories;
using Ticketing.API.Validations;
using Ticketing.API.Repositories.Interfaces;

namespace Ticketing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FrontendController : ControllerBase
    {
        private readonly ITicketRepository ticketRepository;

        public FrontendController(ITicketRepository ticketRepository)
        {
            this.ticketRepository = ticketRepository;
        }

        [HttpPost]
        //[Authorize]
        [ValidateModel]
        [Route("ticket")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] TicketRequestDto ticketRequest)
        {
            try
            {
                var ticket = await ticketRepository.Create(ticketRequest);
                return Ok(new ApiResponse<TicketResponseDto>()
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

        [HttpGet]
        [Authorize]
        [Route("ticket")]
        public async Task<IActionResult> GetUserTickets([FromQuery] string userName , int pageNo = 1, int pageSize= 10)
        {
            try
            {
                var data = await ticketRepository.GetUserTickets(userName, pageNo, pageSize);
                return Ok(new ApiResponse<PaginatedModel<TicketResponseDto>>()
                {
                    Success = true,
                    Message = "Data Fetched Successfully",
                    Data = data
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
